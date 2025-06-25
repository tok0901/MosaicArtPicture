using Android.App;
using Android.Content;
using Android.OS;
using Android.Graphics;
using Android.Widget;
using System.Collections.Generic;
using AndroidX.AppCompat.App;
using System;

namespace App2
{
    [Activity(Label = "モザイクアート", LaunchMode = Android.Content.PM.LaunchMode.SingleInstance, MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        //保存しておく写真サイズ
        int img_save_w = 1024;

        //モザイクで集める画像のURIの格納
        List<Android.Net.Uri> selected_fileuris = new List<Android.Net.Uri>();

        Bitmap Mosaic_moto_img = null;
    }
}
