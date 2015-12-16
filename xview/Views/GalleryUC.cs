using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using xview.utils;
using System.IO;
using DevExpress.XtraBars.Ribbon;

namespace xview.Views
{
    public partial class GalleryUC : UserControl
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(GalleryUC));

        private static readonly List<string> searchStrs = new List<string>() { @"*.bmp", @"*.jpg", @"*.jpeg", @"*.png"};

        public delegate void OpenImageEventHandler(object sender, string fileName);
        public event OpenImageEventHandler OpenImage;
        protected virtual void OnOpenImage(string fileName)
        {
            if (OpenImage != null)
            {
                OpenImage(this, fileName);
            }
        }

        public GalleryUC()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            try
            {
	            this._galleryControl.Gallery.Orientation = Orientation.Vertical;
	            this._galleryControl.Gallery.ItemImageLayout = DevExpress.Utils.Drawing.ImageLayoutMode.ZoomInside;
	            this._galleryControl.Gallery.ImageSize = new System.Drawing.Size(120, 90);
	            this._galleryControl.Gallery.ShowItemText = true;
                _galleryControl.Gallery.Groups.Clear();
                GalleryItemGroup group = new GalleryItemGroup();
                string defaultImageSavePath = ConfigManager.GetAppConfig("WorkPathImage");
                string currentImagePath = defaultImageSavePath;
                buttonEditGalleryImagePath.Text = defaultImageSavePath;
                group.Caption = currentImagePath;
                _galleryControl.Gallery.Groups.Add(group);
                ReLoadImageFilesToGallery();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void ReLoadImageFilesToGallery()
        {
            try
            {
                string currentImagePath = buttonEditGalleryImagePath.Text + "\\";    //ConfigManager.GetAppConfig("WorkPathImage");
	            DirectoryInfo theFolder = new DirectoryInfo(currentImagePath);
	            List<FileInfo> fileList = new List<FileInfo>();
	            foreach (string searchStr in searchStrs)
	            {
	                fileList.AddRange(theFolder.GetFiles(searchStr));
	            }
                GalleryItemGroup group = _galleryControl.Gallery.Groups[0];
                group.Items.Clear();
                foreach (FileInfo fileInfo in fileList)
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(fileInfo.FullName);
                    GalleryItem item = new GalleryItem(image, fileInfo.Name, "");
                    item.Tag = fileInfo.FullName;
                    group.Items.Add(item);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occurred in LoadImageFilesToGallery:" + ex.Message);
            }
        }
        
        /// <summary>
        /// 刷新Gallery
        /// </summary>
        public void RefreshGallery()
        {
            ReLoadImageFilesToGallery();
        }

        /// <summary>
        /// 增加一副图片到Gallery
        /// </summary>
        /// <param name="fullImageName">全路径名</param>
        /// <param name="fileName">文件名</param>
        public void AddImageToGallery(string fullImageName, string fileName)
        {
            try
            {
	            GalleryItemGroup group = _galleryControl.Gallery.Groups[0];
	            System.Drawing.Image image = System.Drawing.Image.FromFile(fullImageName);
	            GalleryItem item = new GalleryItem(image, fileName, "");
	            item.Tag = fullImageName;
	            group.Items.Add(item);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void galleryControlGallery1_ItemClick(object sender, GalleryItemClickEventArgs e)
        {
            try
            {
                string fullName = e.Item.Tag as string;
            	OnOpenImage(fullName);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void buttonEditGalleryImagePath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                FolderBrowserDialog folderDig = new FolderBrowserDialog();
                if (folderDig.ShowDialog() == DialogResult.OK)
                {
                    buttonEditGalleryImagePath.Text = folderDig.SelectedPath;
                    ReLoadImageFilesToGallery();
                    GalleryItemGroup group = _galleryControl.Gallery.Groups[0];
                    group.Caption = buttonEditGalleryImagePath.Text;
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
    }
}
