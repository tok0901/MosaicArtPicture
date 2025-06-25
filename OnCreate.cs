protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout2);

            // ストレージの読み書き権限の確認
            if (Build.VERSION.SdkInt > BuildVersionCodes.Q)
            {   //Android11.0以上の場合のみ
                System.Collections.Generic.List<string> Manifest_Permissions = new System.Collections.Generic.List<string>();

                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Tiramisu)
                {
                    //Android13.0以上の場合
                    //https://learn.microsoft.com/en-us/answers/questions/1354992/xamarin-android-13-popup-permission-notification-a
                    Manifest_Permissions.Add(Android.Manifest.Permission.PostNotifications);
                    Manifest_Permissions.Add(Android.Manifest.Permission.ReadMediaImages);
                    Manifest_Permissions.Add(Android.Manifest.Permission.AccessMediaLocation);
                }
                else
                {
                    Manifest_Permissions.Add(Android.Manifest.Permission.WriteExternalStorage);
                    Manifest_Permissions.Add(Android.Manifest.Permission.ReadExternalStorage);
                    Manifest_Permissions.Add(Android.Manifest.Permission.AccessMediaLocation);
                }

                //各権限をループ2
                foreach (System.String Permission_str in Manifest_Permissions)
                {
                    //https://docs.microsoft.com/ja-jp/xamarin/android/app-fundamentals/permissions?tabs=windows
                    //https://www.petitmonte.com/java/android_fileprovider.html
                    if (ApplicationContext.CheckCallingOrSelfPermission(Permission_str) !=
                        Android.Content.PM.Permission.Granted)
                    {   //許可されていない場合
                        // ストレージの権限の許可を求めるダイアログを表示する
                        //https://qiita.com/khara_nasuo486/items/f23c91ccd37db885aefe
                        if (AndroidX.Core.App.ActivityCompat.ShouldShowRequestPermissionRationale(this,
                                Permission_str))
                        {
                            AndroidX.Core.App.ActivityCompat.RequestPermissions(this,
                                    Manifest_Permissions.ToArray(), (int)Android.Content.PM.RequestedPermission.Required);

                        }
                        else
                        {
                            Toast toast =
                                    Toast.MakeText(ApplicationContext, "アプリ実行の権限が必要です", ToastLength.Short);
                            toast.Show();

                            AndroidX.Core.App.ActivityCompat.RequestPermissions(this,
                                    Manifest_Permissions.ToArray(),
                                    (int)Android.Content.PM.RequestedPermission.Required);

                        }
                    }

                }
            }


            Button btnMosaicstart = FindViewById<Button>(Resource.Id.btnMosaicstart);
            btnMosaicstart.Click += delegate
            {
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetCancelable(false); //ダイアログ外クリックでのキャンセル禁止
                alert.SetTitle("モザイクアート作成開始の確認");
                alert.SetMessage("モザイクアートで集めて表現する元の画像素材を選択してください。");
                alert.SetPositiveButton("はい", (senderAlert, args) =>
                {
                    //素材画像の選択
                    image_select_main(0, 0, this);

                });
                alert.SetNegativeButton("いいえ", (senderAlert, args) =>
                {   //何もしない
                    return;
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            };
        }
