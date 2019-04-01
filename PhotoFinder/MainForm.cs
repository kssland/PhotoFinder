using Newtonsoft.Json;
using PhotoFinder.Data;
using PhotoFinder.Unsplash;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            string inputKeyword = txtKeyword.Text.Trim();
            if (inputKeyword.Length <= 0)
            {
                MessageBox.Show("Please enter a keyword!");
                return;
            }
            btnSearchPhoto.Enabled = false;
            GetPhotosByKeyword(inputKeyword);
        }

        // 서버 API 호출 시작
        private async void GetPhotosByKeyword(string keyword)
        {
            await DoPhotoSearch(keyword);
        }

        // 서버로부터 받은 사진 리스트를 ListView 컨트롤에 삽입하고 Display 한다
        private async Task DoPhotoSearch(string keyword)
        {
            if (!keyword.Equals(lastKeyword))
            {
                photoListView.Clear();
                photoListView.LargeImageList = null;
                page = 1;
            }
            ResultData resultData = await unsplash.GetPhotoListByKeyword(keyword, page);

            if (resultData.Result == RESULT.SUCCEED)
            {
                List<Photo> photoList = unsplash.MakePhotoItemList((string)resultData.obj);

                ImageList imageList;
                if (photoListView.LargeImageList == null)
                {
                    imageList = new ImageList();
                    imageList.ImageSize = new Size(200, 200);
                    imageList.ColorDepth = ColorDepth.Depth32Bit;
                    photoListView.LargeImageList = imageList;
                }
                imageList = photoListView.LargeImageList;

                int count = photoListView.Items.Count;
                foreach (Photo item in photoList)
                {
                    ResultData thumbnailData = await unsplash.DownloadStream(item.ThumbnailDownloadUrl);
                    if (thumbnailData.Result == RESULT.SUCCEED)
                    {
                        Stream stream = (Stream)thumbnailData.obj;
                        Bitmap bitmap = new Bitmap(stream);
                        stream.Flush();
                        stream.Close();
                        imageList.Images.Add(bitmap);

                        ListViewItem lvItem = new ListViewItem();
                        lvItem.Text = item.Description;
                        if (lvItem.Text.Length == 0)
                            lvItem.Text = "No description";
                        lvItem.Text += "\n좋아요: " + item.Likes;
                        lvItem.ToolTipText = item.ToolTipText;
                        lvItem.ImageIndex = count++;                        
                        photoListView.Items.Add(lvItem);
                    }
                }

                lastKeyword = keyword;
                page++;

                dynamic jobj = JsonConvert.DeserializeObject((string)resultData.obj);
                int totalCount = jobj.total;
                string countText = "사진 검색\n" + "(" + photoListView.Items.Count.ToString() + "/" + totalCount + ")";
                btnSearchPhoto.Text = countText;
            }            
            btnSearchPhoto.Enabled = true;
        }
    }
}
