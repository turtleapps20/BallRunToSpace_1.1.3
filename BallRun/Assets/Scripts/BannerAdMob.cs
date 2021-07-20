using UnityEngine;
using GoogleMobileAds.Api;

public class BannerAdMob : MonoBehaviour
{
    private BannerView bannerView;

    string adUnitId = "ca-app-pub-3076913227168198/7572075241";

    private void OnEnable()
    {
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
    }
}
