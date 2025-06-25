 public async System.Threading.Tasks.Task<Bitmap> Image_file_making(int image_no, int page_no, Activity ac, Intent data, Button lblsyori)
        {   //画像差し込み処理
            int err_cnt = 500;
            lblsyori.Text = "[No." + err_cnt.ToString() + "]画像準備中....";
            await System.Threading.Tasks.Task.Delay(10);

            try
            {   //画像を取得
                //https://stackoverflow.com/questions/20013220/translate-android-java-to-xamarin-c-sharp
                //https://stackoverflow.com/questions/43753989/xamarin-android-image-uri-to-byte-array


                //jpegの場合⇒拡張子が付いてくるとは限らない。
                //Exifに応じた回転情報を探る
                lblsyori.Text = "[No." + err_cnt.ToString() + "]画像情報取得中";
                await System.Threading.Tasks.Task.Delay(10);
                Bitmap bmp_main = null; //初期化

                using (var inputStream = ac.ContentResolver.OpenInputStream(data.Data))
                {
                    //事前準備
                    lblsyori.Text = "[No." + err_cnt.ToString() + "]画像ロード中";

                    //画像の読出し
                    bmp_main = BitmapFactory.DecodeStream(inputStream, null, null);
                    inputStream.Close();
                    err_cnt = 503;
                }

                //リサイズする。
                bmp_main = bmp_size_format(bmp_main, img_save_w, img_save_w, false);
                err_cnt = 505;

                lblsyori.Text = "[No." + err_cnt.ToString() + "]画像保存中";
                await System.Threading.Tasks.Task.Delay(10);
                return bmp_main;
            }
            catch
            {
                Toast.MakeText(Application.Context, "選択した画像は、存在していないか、読み込みに失敗しました。:" + err_cnt.ToString(), ToastLength.Long).Show();

                return null;
            }
        }
