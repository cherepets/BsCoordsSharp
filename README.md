# BsCoordsSharp

Library to get location from cellural networks information (MCC, MNC, LAC, CellID) using Google location services, Yandex location services, OpenCellID, Mylnikov Geo and Mozilla Location Service.
Used bscoords library by Alexander Zubakov as a reference.

# Usage

```C#
await BsGoogleClient().RequestAsync(mcc, mnc, lac, cid);
await BsMozillaClient(apiKey).RequestAsync(mcc, mnc, lac, cid);
await BsMylnikovClient().RequestAsync(mcc, mnc, lac, cid);
await BsOpenCellIdClient(apiKey).RequestAsync(mcc, mnc, lac, cid);
await BsYandexClient().RequestAsync(mcc, mnc, lac, cid);
```

[Download from NuGet Gallery](https://www.nuget.org/packages/FourToolkit.Charts/ "Download from NuGet Gallery")

See also: [kolonist/bscoords](https://github.com/kolonist/bscoords "kolonist/bscoords")

License: [WTFPL](http://www.wtfpl.net/txt/copying/ "WTFPL")
