public async System.Threading.Tasks.Task bitmap_hontai_save(Bitmap bitmap, string filePath, Button lblsyori)
        {   //本体のPictureフォルダに画像の保存
            try
            {
                //画像保存
                //https://garakutatech.blogspot.com/2021/02/androidmedia-store.html
                //https://codechacha.com/ja/android-mediastore-insert-media-files/

                ContentValues values = new ContentValues();
                // コンテンツ クエリの列名

                values.Put(Android.Provider.MediaStore.Images.Media.InterfaceConsts.RelativePath, "Pictures/TageSP");
                // ファイル名
                values.Put(Android.Provider.MediaStore.Images.Media.InterfaceConsts.DisplayName, System.IO.Path.GetFileName(filePath));

                // マイムの設定
                //https://developer.mozilla.org/ja/docs/Web/Media/Formats/Image_types
                //JPEG保存の場合
                values.Put(Android.Provider.MediaStore.Images.Media.InterfaceConsts.MimeType, "image/jpeg");

                // 書込み時にメディア ファイルに排他的にアクセスする
                values.Put(Android.Provider.MediaStore.Images.Media.InterfaceConsts.IsPending, 1);


                ContentResolver resolver = ContentResolver;
                Android.Net.Uri collection = Android.Provider.MediaStore.Images.Media.GetContentUri(Android.Provider.MediaStore.VolumeExternalPrimary);
                Android.Net.Uri item = resolver.Insert(collection, values);

                //ファイル書き込み
                lblsyori.Text = "画像保存中";
                using (System.IO.Stream outstream = resolver.OpenOutputStream(item))
                {
                    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, outstream);

                    //https://stackoverflow.com/questions/71794933/how-to-save-a-bitmap-on-storage-in-android-q-and-later
                    outstream.Flush();
                    outstream.Close();
                };

                //　排他的にアクセスの解除
                values.Clear();
                values.Put(Android.Provider.MediaStore.Images.Media.InterfaceConsts.IsPending, 0);
                resolver.Update(item, values, null, null);
                await System.Threading.Tasks.Task.Delay(10);

                lblsyori.Text = "画像保存完了";
                return;
            }
            catch
            {
                return; //出る
            }
        }
