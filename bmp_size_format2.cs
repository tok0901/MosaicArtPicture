public Bitmap bmp_size_format2(Bitmap bmp, int max_w, int max_h)
        {   //単純に指定サイズにリサイズを行う
            //安全装置付き
            if (max_w > 30 && max_h > 30)
            {   //リサイズ画像の作成
                //http://seesaawiki.jp/w/moonlight_aska/d/BMP%B2%E8%C1%FC%A4%F2%A5%EA%A5%B5%A5%A4%A5%BA%A4%B9%A4%EB
                try
                {
                    //まずは正攻法での縮小を試みる
                    return Bitmap.CreateScaledBitmap(bmp, max_w, max_h, true);
                }
                catch
                {   //失敗時→別の方法を試す
                    //元地の画像作成
                    Bitmap Haikei = Bitmap.CreateBitmap(max_w, max_h, Android.Graphics.Bitmap.Config.Argb8888);

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
                            Rect dst = new Rect(0, 0, max_w, max_h);

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
