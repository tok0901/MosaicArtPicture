public Bitmap bmp_size_format(Bitmap bmp, int max_w, int max_h, bool force_resize)
        {   //画像の最大サイズを大きく超える画像は、
            //動作不良防止のため、予め小さくして扱う
            int w = bmp.Width;
            int h = bmp.Height;
            bool resize_flg = false; //リサイズが必要かどうか？


            if (bmp.Width >= bmp.Height && (bmp.Width > max_w || force_resize))
            { //横長の画像で、(最大サイズを超えた場合、または、強制リサイズ時)
                resize_flg = true;//リサイズ必要
                w = max_w;
                h = (w * bmp.Height) / bmp.Width;
                if (h > max_h)
                { //上限値補正
                    h = max_h;
                    w = (h * bmp.Width) / bmp.Height;
                }
            }
            else if (bmp.Height > max_h || force_resize)
            {   //縦長の画像で、最大サイズを超えた場合、または、強制リサイズ時
                resize_flg = true;//リサイズ必要
                h = max_h;
                w = (h * bmp.Width) / bmp.Height;
                if (w > max_w)
                { //上限値補正
                    w = max_w;
                    h = (w * bmp.Height) / bmp.Width;
                }
            }

            //リサイズが必要な場合
            //安全装置付き
            if (resize_flg && w > 0 && h > 0)
            {   //リサイズ画像の作成
                //http://seesaawiki.jp/w/moonlight_aska/d/BMP%B2%E8%C1%FC%A4%F2%A5%EA%A5%B5%A5%A4%A5%BA%A4%B9%A4%EB
                try
                {
                    //まずは正攻法での縮小を試みる
                    return Bitmap.CreateScaledBitmap(bmp, w, h, true);
                }
                catch
                {   //失敗時→別の方法を試す
                    //元地の画像作成
                    Bitmap Haikei = Bitmap.CreateBitmap(w, h, Android.Graphics.Bitmap.Config.Argb8888);

                    //描画開始
                    using (Android.Graphics.Canvas canvas = new Android.Graphics.Canvas(Haikei))
                    {
                        using (var paint = new Paint())
                        {   //高画質で縮小させる。
                            //https://qiita.com/t2low/items/33606d6403226965b3bf
                            //http://anadreline.blogspot.com/2013/07/android_3.html
                            paint.FilterBitmap = true; //画像をきれいに縮小??

                            // 描画元の矩形イメージ
                            Rect src = new Rect(0, 0, bmp.Width, bmp.Height);
                            // 描画先の矩形イメージ
                            Rect dst = new Rect(0, 0, w, h);

                            //座標に指定のサイズで表示する
                            //https://dev.classmethod.jp/smartphone/xamarin-android-draw-image/
                            //そのままのDrawBitmapでのリサイズは、画質が劣化する。
                            //https://dev.classmethod.jp/smartphone/xamarin-android-draw-image/
                            canvas.DrawBitmap(bmp, src, dst, paint);
                        }
                    }
                    //ここで、Disposeはしてはいけない。
                    return Haikei;
                }
            }
            else
            {   //そのまま返す
                return bmp;
            }

        }
