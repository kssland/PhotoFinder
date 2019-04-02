using Newtonsoft.Json;
using PhotoFinder.Data;
using PhotoFinder.Unsplash;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;

namespace PhotoFinder
{
    public partial class MainForm : Form
    {
        UnsplashRestAPI unsplash = new UnsplashRestAPI();
        int page = 1;
        string lastKeyword = "";

        public MainForm()
        {
            InitializeComponent();
            photoListView.ShowItemToolTips = true;
        }

        // 검색 버튼 클릭
        private void btnSearchPhoto_Click(object sender, EventArgs e)
        {
            if (!ValidateKeywordInput())
                return;
            SearchPhotosByKeyword();
        }

        // 키워드 입력 박스에서 엔터 입력 시
        private void tbKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!ValidateKeywordInput())
                    return;
                SearchPhotosByKeyword();
            }
        }

        // 키워드 입력 체크
        private bool ValidateKeywordInput()
        {
            string inputKeyword = tbKeyword.Text.Trim();
            if (inputKeyword.Length <= 0)
            {
                MessageBox.Show("검색어를 입력해 주세요!");
                return false;
            }
            return true;
        }

        // 사진 검색 시작
        private async void SearchPhotosByKeyword()
        {
            btnSearchPhoto.Enabled = false;
            await SearchPhotoTask(tbKeyword.Text.Trim());
        }

        // 서버로부터 받은 사진 리스트를 ListView 컨트롤에 삽입하고 Display 한다
        private async Task SearchPhotoTask(string keyword)
        {
            // 새로운 검색어가 입력 된 경우 리스트뷰 초기화
            if (!keyword.Equals(lastKeyword))
                InitPhotoListView();

            // 서버에서 키워드로 검색된 사진 리스트를 가져온다
            ResponseData responseData = await unsplash.GetPhotoListByKeyword(keyword, page);
            if (responseData.Result == RESULT.SUCCEED)
            {
                // Json 데이터를 사진 아이템으로 변환한 리스트를 만든다
                List<Photo> photoItemList = MakePhotoItemList((string)responseData.Obj);
                foreach (Photo photo in photoItemList)
                {
                    // 썸네일 이미지 다운로드
                    ResponseData thumbnailData = await unsplash.DownloadStream(photo.ThumbnailUrl);
                    // 썸네일 이미지 리스트뷰에 삽입
                    if (thumbnailData.Result == RESULT.SUCCEED)
                        InsertPhotoToListView((Stream)thumbnailData.Obj, photo);
                    else
                        MessageBox.Show(responseData.Obj.ToString());
                }

                // 검색 버튼 텍스트를 갱신(현재 다운로드 개수/전체 개수)
                dynamic jobj = JsonConvert.DeserializeObject((string)responseData.Obj);                
                UpdateSearchBtnText((int)jobj.total);
                
                this.lastKeyword = keyword; // 사용자가 입력한 마지막 키워드 저장                
                this.page++; // 다음에 가져올 페이지 번호 업데이트
            }
            else
                MessageBox.Show(responseData.Obj.ToString());

            btnSearchPhoto.Enabled = true;
        }

        // 검색된 사진 리스트(Json)를 Photo 아이템으로 변환 후 리스트로 만들어주는 메서드
        private List<Photo> MakePhotoItemList(string jsonStr)
        {
            dynamic jobj = JsonConvert.DeserializeObject(jsonStr);
            List<Photo> photoList = new List<Photo>();
            foreach (var item in jobj.results)
            {
                Photo photo = new Photo();
                photo.Id = item.id;
                photo.Description = item.description;
                photo.UserName = item.user.username;
                photo.CreatedTime = item.created_at;
                photo.UpdatedTime = item.updated_at;
                photo.ThumbnailUrl = item.urls.thumb;
                photo.FullImageUrl = item.urls.full;
                photo.Width = item.width;
                photo.Height = item.height;
                photo.Likes = item.likes;
                photo.ToolTipText = item.alt_description;
                photoList.Add(photo);
            }
            return photoList;
        }

        // 사진을 리스트뷰에 삽입
        private void InsertPhotoToListView(Stream imageStream, Photo photo)
        {
            Bitmap thumbnail = new Bitmap(imageStream);
            imageStream.Flush();
            imageStream.Close();
            GetPhotoImageList().Images.Add(thumbnail);

            ListViewItem lvItem = new ListViewItem();
            lvItem.Text = photo.Description;
            if (lvItem.Text.Length == 0)
                lvItem.Text = "No description";
            lvItem.Text += "\n좋아요: " + photo.Likes;
            lvItem.ToolTipText = photo.ToolTipText;
            lvItem.ImageIndex = photoListView.Items.Count;
            lvItem.Tag = photo.FullImageUrl;
            this.photoListView.Items.Add(lvItem);
        }

        // 리스트뷰 초기화
        private void InitPhotoListView()
        {
            this.photoListView.Clear();
            this.photoListView.LargeImageList = null;
            this.page = 1;
        }

        // 리스트뷰의 LargeImageList 리턴
        private ImageList GetPhotoImageList()
        {
            if (this.photoListView.LargeImageList == null)
            {
                this.photoListView.LargeImageList = new ImageList();
                this.photoListView.LargeImageList.ImageSize = new Size(200, 200);
                this.photoListView.LargeImageList.ColorDepth = ColorDepth.Depth32Bit;
            }
            return this.photoListView.LargeImageList;
        }

        // 검색 버튼 텍스트 업데이트
        private void UpdateSearchBtnText(int totalCount)
        {
            string countText = "사진 검색\n" + "(" + photoListView.Items.Count.ToString() + "/" + totalCount + ")";
            this.btnSearchPhoto.Text = countText;
        }

        // 원본 이미지 다운로드 기능
        private void photoListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                object tmp = photoListView.SelectedItems;
                if (photoListView.SelectedItems.Count == 1)
                {
                    SelectedListViewItemCollection selectedItems = photoListView.SelectedItems;
                    ListViewItem lvItem = selectedItems[0];
                    ContextMenu contextMenu = new ContextMenu();
                    MenuItem menuItem = new MenuItem();
                    menuItem.Text = "원본 다운로드";
                    menuItem.Click += async (senders, es) =>
                    {
                        SaveFileDialog savePanel = new SaveFileDialog();
                        savePanel.InitialDirectory = @"c:\";
                        savePanel.Filter = "JPG File (*.jpg)|*.jpg";
                        if (savePanel.ShowDialog() == DialogResult.OK)
                        {
                            using (var saveFileStream = File.Create(savePanel.FileName))
                            {
                                ResponseData fullImageData = await unsplash.DownloadStream((string)lvItem.Tag);
                                if (fullImageData.Result == RESULT.SUCCEED)
                                {
                                    Stream fullImageStream = (Stream)fullImageData.Obj;
                                    fullImageStream.CopyTo(saveFileStream);
                                }
                                else
                                    MessageBox.Show(fullImageData.Obj.ToString());
                            }                            
                        }                        
                    };

                    contextMenu.MenuItems.Add(menuItem);
                    contextMenu.Show(photoListView, e.Location);
                }
            }
        }
    }
}
