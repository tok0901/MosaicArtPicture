public async System.Threading.Tasks.Task collage_Start_Sub(Activity ac, List<Android.Net.Uri> selected_fileuris, Button lblsyori)
        {   //モザイク画像を作成するところ

            try
            {
                BitmapFactory.Options options = new BitmapFactory.Options();
                lblsyori.Text = "再現元の画像の取得中";
                await System.Threading.Tasks.Task.Delay(10);

                //縮小サイズの取得
                int load_int_chou = 128;

                //元画像をリサイズする。
                Bitmap bmp_main = bmp_size_format(Mosaic_moto_img, load_int_chou, load_int_chou, false);
                options.Dispose();

                //Bitmapのバイナリ置き場の準備
                //https://qiita.com/aymikmts/items/7139fa6c4da3b57cb4fc
                Java.Nio.ByteBuffer byteBuffer2 = Java.Nio.ByteBuffer.Allocate(bmp_main.ByteCount);
                bmp_main.CopyPixelsToBuffer(byteBuffer2);
                byteBuffer2.Flip();

                //基礎Bitmapのバイナリの格納
                byte[] bmparr = new byte[byteBuffer2.Capacity()];
                byteBuffer2.Duplicate().Get(bmparr);
                byteBuffer2.Clear();

                lblsyori.Text = "再現元のピクセル取得中";
                await System.Threading.Tasks.Task.Delay(10);

                //画像の各ピクセルの色を格納
                List<Android.Graphics.Color> moto_img_Color = new List<Android.Graphics.Color>();
                moto_img_Color.Clear();
                int base_cnt = 0;
                do
                {   //各ピクセルの色を取得していく
                    //安全装置
                    if (bmparr.Length == base_cnt) { break; }

                    //色を格納する
                    moto_img_Color.Add(Android.Graphics.Color.Rgb(bmparr[base_cnt], bmparr[base_cnt + 1], bmparr[base_cnt + 2]));

                    //次へ
                    base_cnt += 4;
                } while (bmparr.Length > base_cnt); //最後までループ



                //モザイク画として配置する画像の取得
                List<Android.Graphics.Bitmap> selected_Bitmap = new List<Android.Graphics.Bitmap>();
                selected_Bitmap.Clear();

                //ARGB8888にて扱う。
                Android.Graphics.Bitmap.Config bitmapConfig = Android.Graphics.Bitmap.Config.Argb8888;

                //集める画像の小さなサイズのものを取得
                int load_int_ippen = 32;
                base_cnt = 0;
                foreach (Android.Net.Uri selected_saki_imguri in selected_fileuris)
                {   //各個小さな画像をリストにして入れている
                    try
                    {
                        using (var inputStream = ac.ContentResolver.OpenInputStream(selected_saki_imguri))
                        {
                            //事前準備
                            if (base_cnt % 3 == 0)
                            {
                                lblsyori.Text = "集める画像を取得中" + base_cnt.ToString();
                                await System.Threading.Tasks.Task.Delay(10);
                            }

                            //画像の読出し
                            Android.Graphics.Bitmap bmp_main2 = BitmapFactory.DecodeStream(inputStream, null, null);
                            options.Dispose();
                            inputStream.Close();

                            //元画像をリサイズしてリストに格納する。
                            bmp_main2 = bmp_size_format2(bmp_main2, load_int_ippen, load_int_ippen);

                            bmp_main2 = bmp_main2.Copy(bitmapConfig, true);

                            selected_Bitmap.Add(bmp_main2);
                        }
                        base_cnt += 1;
                    }
                    catch { }
                }

                //与えられた画像たちの平均色を設置する。

                //モザイク画として配置する画像の色とした場合どんな色となるかを取得
                List<Android.Graphics.Color> selected_Color = new List<Android.Graphics.Color>();
                selected_Color.Clear();
                int midx = 0;

                foreach (Android.Graphics.Bitmap bmp_main2 in selected_Bitmap)
                {
                    if (midx % 50 == 0)
                    {
                        lblsyori.Text = "集める画像のピクセル取得中" + midx.ToString();
                        await System.Threading.Tasks.Task.Delay(10);
                    }
                    //Bitmapのバイナリ置き場の準備
                    //https://qiita.com/aymikmts/items/7139fa6c4da3b57cb4fc
                    Java.Nio.ByteBuffer byteBuffer = Java.Nio.ByteBuffer.Allocate(bmp_main2.ByteCount);
                    bmp_main2.CopyPixelsToBuffer(byteBuffer);
                    byteBuffer.Flip();

                    //基礎Bitmapのバイナリの格納
                    bmparr = new byte[byteBuffer.Capacity()];
                    byteBuffer.Duplicate().Get(bmparr);
                    byteBuffer.Clear();

                    //初期化
                    base_cnt = 0;
                    long av_r = 0;
                    long av_g = 0;
                    long av_b = 0;
                    long av_cnt = 0;

                    do
                    {
                        //安全装置
                        if (bmparr.Length == base_cnt) { break; }

                        //色の合計を取得する
                        av_r += (long)bmparr[base_cnt];
                        av_g += (long)bmparr[base_cnt + 1];
                        av_b += (long)bmparr[base_cnt + 2];
                        //int color_a = bmparr[base_cnt + 3];

                        //次へ
                        base_cnt += 4;
                        av_cnt += 1; //ピクセル数をカウント
                    } while (bmparr.Length > base_cnt); //最後までループ

                    //この画像の平均色を取得
                    av_r = av_r / av_cnt;
                    av_g = av_g / av_cnt;
                    av_b = av_b / av_cnt;
                    selected_Color.Add(Android.Graphics.Color.Rgb((int)av_r, (int)av_g, (int)av_b));
                }


                //出力画像サイズの算出
                int hw = bmp_main.Width * load_int_ippen; //画像幅
                int hh = bmp_main.Height * load_int_ippen; //画像高さ
                //元地の画像作成
                Bitmap Haikei = Android.Graphics.Bitmap.CreateBitmap(hw, hh, bitmapConfig);
                midx = 0;
                using (Android.Graphics.Canvas canvas = new Android.Graphics.Canvas(Haikei))
                {
                    using (var paint = new Paint())
                    {
                        foreach (Android.Graphics.Color mclr in moto_img_Color)
                        {   //元画像の各ピクセルに一番近い色の画像をはめ込んでいく
                            if (midx % 100 == 0)
                            {
                                lblsyori.Text = "集める画像で構成中" + midx.ToString();
                                await System.Threading.Tasks.Task.Delay(10);
                            }
                            //一番近い色を抽出する。
                            int min_span = 99999;
                            int min_span_idx = -1;
                            base_cnt = 0;
                            foreach (Android.Graphics.Color sclr in selected_Color)
                            {
                                int span = Math.Abs(mclr.R - sclr.R); //赤の色差
                                span += Math.Abs(mclr.G - sclr.G); //赤の色差
                                span += Math.Abs(mclr.G - sclr.B); //青の色差
                                if (span < min_span)
                                {
                                    //より小さい色差が見つかった場合
                                    min_span = span; //候補を変更する。
                                    min_span_idx = base_cnt;
                                }
                                base_cnt += 1;
                            }

                            //最も近かった画像のサムネイルを下地に描画する。
                            int left = (midx % bmp_main.Width) * load_int_ippen;
                            int top = (midx / bmp_main.Width) * load_int_ippen;
                            paint.AntiAlias = true;
                            canvas.DrawBitmap(selected_Bitmap[min_span_idx], left, top, paint);
                            midx += 1;
                        }
                    }
                }

                lblsyori.Text = "処理終了中";
                await System.Threading.Tasks.Task.Delay(10);

                //画像などの解放
                foreach (Android.Graphics.Bitmap bmp_main2 in selected_Bitmap)
                {
                    bmp_main2.Dispose();
                }
                selected_Bitmap.Clear();
                bmp_main.Dispose();

                if (Haikei != null)
                {   //画像を保存する場合
                    await bitmap_hontai_save(Haikei, "test.jpg", lblsyori);

                    Haikei.Dispose();
                    Mosaic_moto_img.Dispose();
                }
            }
            catch { }
            return;
        }
