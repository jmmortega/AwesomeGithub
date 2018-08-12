using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using AwesomeGithub.Control;
using AwesomeGithub.Droid.Render;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:Xamarin.Forms.ExportRenderer(typeof(LabelTLDR), typeof(LabelTLDRRender))]
namespace AwesomeGithub.Droid.Render
{
    public class LabelTLDRRender : LabelRenderer
    {
        public LabelTLDRRender(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if(Control != null)
            {
                Control.SetMaxLines(2);
                Control.Ellipsize = TextUtils.TruncateAt.End;
            }
        }


    }
}