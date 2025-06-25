public void image_select_main(int no, int requestCode, Activity ac)
        {   //画像選択時の処理
            
            try
            {
                string caption = "";

                //https://developer.xamarin.com/recipes/android/data/files/selecting_a_gallery_image/
                var imageIntent = new Intent(Intent.ActionGetContent);
                if (no == -1)
                {   //素材画像
                    imageIntent.SetType("image/*");
                    caption = "モザイクアートの集合の素材となる画像を選択してください。";
                }
                else
                {   //元画像
                    imageIntent.SetType("image/*");
                    caption = "モザイクアートの元となる画像を選択してください。";
                }

                //複数画像選択かどうか？0なら複数選択
                //https://stackoverflow.com/questions/19585815/select-multiple-images-from-android-gallery
                imageIntent.PutExtra(Intent.ExtraAllowMultiple, (no == -1));

                imageIntent.SetAction(Intent.ActionGetContent);
                ac.StartActivityForResult(
                    Intent.CreateChooser(imageIntent, caption), requestCode);
            }
            catch
            {   //エラー時
                Toast.MakeText(Application.Context, "選択ダイアログエラー", ToastLength.Long).Show();
            }
        }
