using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine;

public class Banner : MonoBehaviour
{
    private BannerView bannerView;
    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });

        this.RequestBanner();
    }

    public void RequestBanner()
    {
        #if UNITY_ANDROID
        string _adUnitId = "";
        #elif UNITY_IPHONE
        string _adUnitId = "";
        #else
        string _adUnitId = "unused";
        #endif

        this.bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Top);

        AdRequest request = new AdRequest.Builder().Build();
        this.bannerView.LoadAd(request);

    }
}