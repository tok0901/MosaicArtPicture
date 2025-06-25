protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            int err_cnt = 0;
            try
            {
                Button btnMosaicstart = FindViewById<Button>(Resource.Id.btnMosaicstart);
                string bf_text = btnMosaicstart.Text;


                //◆◆◆以下、画像選択時のイベント◆◆◆
                if (resultCode == Result.Ok)
                {   //OK(ズドン)
                    if (requestCode == 0)
                    {   //元画像の選択
                        try
                        {   //画像の埋め込み処理
                            Mosaic_moto_img = await Image_file_making(0, 0, this, data, btnMosaicstart);

                            //文字を戻す
                            btnMosaicstart.Text = bf_text;

                            //素材画像の選択
                            image_select_main(-1, 1, this);
                        }
                        catch
                        {
                            Toast.MakeText(ApplicationContext, "選択した写真は、存在していないか、読み込みに失敗しました。:" + err_cnt.ToString(), ToastLength.Long).Show();
                        }
                    }
                    else if (requestCode == 1)
                    {   //集合画像の選択
                        selected_fileuris.Clear();

                        if (data.ClipData != null)
                        {   //複数選択された場合
                            int count = data.ClipData.ItemCount;
                            int currentItem = 0;

                            while (currentItem < count)
                            {   //各ファイルのURIを取得
                                currentItem = currentItem + 1;
                                selected_fileuris.Add(data.ClipData.GetItemAt(currentItem - 1).Uri);
                            }

                            await collage_Start_Sub(this, selected_fileuris, btnMosaicstart);

                            //文字を戻す
                            btnMosaicstart.Text = bf_text;
                        }
                        else if (data.Data != null)
                        {   //1つだけの選択時
                            Toast.MakeText(ApplicationContext, "単一の画像の選択ではモザイクアートは作成できません。", ToastLength.Long).Show();
                        }
                    }
                }
            }
            catch
            {
                Toast.MakeText(ApplicationContext, "選択した写真は、存在していないか、読み込みに失敗しました。:" + err_cnt.ToString(), ToastLength.Long).Show();
            }
        }
