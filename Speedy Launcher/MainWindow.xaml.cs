using System;
using System.IO;
using System.Media;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Speedy_Launcher
{
	/// <summary>
	/// MainWindow.xaml etkileşim mantığı
	/// </summary>
	public partial class MainWindow : Window
	{
		private SoundPlayer SesOynatici = new SoundPlayer();
		private SolidColorBrush KirmiziFirca = new SolidColorBrush(Color.FromRgb(255, 0, 0));
		private SolidColorBrush YesilFirca = new SolidColorBrush(Color.FromRgb(0, 255, 0));
		private bool SesAcik = true;
		private Random RNG = new Random();
		private string Operasyonlar = "+-";
		private bool CevapVerildi = false;
		private float Sonuc;
		private int DogruSayisi = 0;
		private int YanlisSayisi = 0;
		private int PuanSayisi = 0;
		
		public MainWindow()
		{
			InitializeComponent();
			MaksGenislik = SystemParameters.MaximizedPrimaryScreenWidth;
			MaksYukseklik = SystemParameters.MaximizedPrimaryScreenHeight;
			ToplamaButonu.BorderBrush = YesilFirca;
			CikarmaButonu.BorderBrush = YesilFirca;
			CarpmaButonu.BorderBrush = KirmiziFirca;
			BolmeButonu.BorderBrush = KirmiziFirca;
		}
		
		private void IslemButonu_Click(object Nesne, RoutedEventArgs OlayBilgisi)
		{
			Button IslemButonu = Nesne as Button;
			string Islem = button.Uid.ToString();
			if (button.BorderBrush == YesilFirca && Operasyonlar.Length > 1 || button.BorderBrush == KirmiziFirca)
			{
				Operasyonlar = button.BorderBrush == YesilFirca ? Operasyonlar.Replace(button.Uid, "") : Operasyonlar + Islem;
				IslemButonu.BorderBrush = IslemButonu.BorderBrush == KirmiziFirca ? YesilFirca : KirmiziFirca;
			}
		}
		
		private void KucultmeButonu_Click(object Nesne, RoutedEventArgs OlayBilgisi) => WindowState = WindowState.Minimized;
		
		private void BuyutmeButonu_Click(object Nesne, RoutedEventArgs OlayBilgisi) => WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
		
		private void KapatmaButonu_Click(object Nesne, RoutedEventArgs OlayBilgisi) => Close();
		
		private void OyunPenceresi_Geri_Click(object Nesne, MouseButtonEventArgs OlayBilgisi)
		{
			OyunPenceresi_Border.Visibility = Visibility.Hidden;
			BaslaticiPenceresi_Border.Visibility = Visibility.Visible;
			DogruSayisi = 0;
			YanlisSayisi = 0;
			PuanSayisi = 0;
		}
		
		private void SpeedyMat_Click(object Nesne, MouseButtonEventArgs OlayBilgisi)
		{
			BaslaticiPenceresi_Border.Visibility = Visibility.Hidden;
			OyunPenceresi_Border.Visibility = Visibility.Visible;
			Oyun.Visibility = Visibility.Hidden;
			Ayarlar.Visibility = Visibility.Visible;
		}
		
		private void SesAyarlayici_Click(object Nesne, MouseButtonEventArgs OlayBilgisi)
		{
			SesAcik = !SesAcik;
			SesSimgesi.Data = SesAcik == false ? Geometry.Parse("M379 895 l-145 -145 -105 0 c-68 0 -109 -4 -117 -12 -17 -17 -17 -429 0 -446 8 -8 50 -12 123 -12 l110 0 140 -140 c77 -77 145 -140 152 -140 6 0 19 7 27 16 14 14 16 73 16 500 0 316 -4 492 -10 505 -6 10 -18 19 -28 19 -9 0 -83 -65 -163 -145z M717 672 c-26 -29 -21 -42 35 -99 l52 -53 -52 -53 c-56 -57 -62 -75 -34 -100 29 -26 42 -21 99 35 l53 52 53 -52 c57 -56 75 -62 100 -34 26 29 21 42 -35 99 l-52 53 52 53 c56 57 62 75 34 100 -29 26 -42 21 -99 -35 l-53 -52 -53 52 c-57 56 -75 62 -100 34z") : Geometry.Parse("M379 895 l-145 -145 -105 0 c-68 0 -109 -4 -117 -12 -17 -17 -17 -429 0 -446 8 -8 50 -12 123 -12 l110 0 140 -140 c77 -77 145 -140 152 -140 6 0 19 7 27 16 14 14 16 73 16 500 0 316 -4 492 -10 505 -6 10 -18 19 -28 19 -9 0 -83 -65 -163 -145z M648 1024 c-13 -12 -9 -91 5 -101 6 -6 29 -13 50 -17 104 -19 216 -106 268 -207 77 -154 47 -341 -73 -462 -52 -52 -136 -96 -199 -104 -50 -7 -61 -21 -57 -75 l3 -43 67 1 c142 1 298 119 376 281 l37 78 0 145 0 145 -37 78 c-75 156 -226 273 -370 284 -34 3 -66 2 -70 -3z M652 758 c-6 -6 -12 -31 -12 -55 0 -39 3 -45 40 -68 51 -32 75 -69 75 -115 0 -46 -24 -83 -75 -115 -37 -23 -40 -29 -40 -69 0 -62 17 -73 78 -51 89 34 162 139 162 235 0 50 -30 126 -64 167 -50 58 -138 97 -164 71z");
		}
		
		private async void CevapKutusu_KeyDown(object Nesne, KeyEventArgs OlayBilgisi)
		{
			if (OlayBilgisi.Key == Key.Enter)
			{
				Keyboard.ClearFocus();
				CevapKutusu.Visibility = Visibility.Hidden;
				TikIsareti.Visibility = Visibility.Hidden;
				bool CevapDogru = Convert.ToSingle(CevapKutusu.Text) == Sonuc ? true : false;
				CevapKutusu.Text = "";
				SoruYazisi.Content = CevapDogru == true ? "Doğru cevap!" : "Yanlış cevap!";
				if (CevapDogru == true)
				{
					DogruSayisi++;
					PuanSayisi++;
				}
				else
				{
					YanlisSayisi++;
					if (f % 3 == 0 && PuanSayisi > 0)
					{
						PuanSayisi--;
					}
				}
				DogruSayisi_Skor.Content = $"Doğru: {t}";
				YanlisSayisi_Skor.Content = $"Yanlış: {f}";
				PuanSayisi_Skor.Content = $"Puan: {p}";
				if (SesAcik == true)
				{
					SesOynatici.SoundLocation = CevapDogru == true ? Path.GetFullPath("SFX/Correct.wav") : Path.GetFullPath("SFX/Incorrect.wav");
					SesOynatici.Play();
				}
				await Task.Delay(SesAcik == true ? (c == true ? 5000 : 1100) : 1000);
				CevapKutusu_Border.Visibility = Visibility.Hidden;
				SoruYazisi.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
				TikIsareti.Visibility = Visibility.Visible;
				CevapKutusu.Visibility = Visibility.Visible;
				CevapVerildi = true;
			};
		}
		
		private async void Checkmark_PreviewMouseLeftButtonDown(object Nesne, MouseButtonEventArgs OlayBilgisi)
		{
			Keyboard.ClearFocus();
			CevapKutusu.Visibility = Visibility.Hidden;
			TikIsareti.Visibility = Visibility.Hidden;
			SoruYazisi.Content = Convert.ToSingle(CevapKutusu.Text) == Sonuc ? "Doğru cevap!" : "Yanlış cevap!";
			CevapKutusu.Text = "";
			if (SesAcik == true)
			{
				SesOynatici.SoundLocation = Convert.ToString(SoruYazisi.Content) == "Doğru cevap!" ? Path.GetFullPath("SFX/Correct.wav") : Path.GetFullPath("SFX/Incorrect.wav");
				SesOynatici.Play();
			}
			await Task.Delay(SesAcik == true ? (Convert.ToString(SoruYazisi.Content) == "Doğru cevap!" ? 5000 : 1100) : 1000);
			CevapKutusu_Border.Visibility = Visibility.Hidden;
			SoruYazisi.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
			TikIsareti.Visibility = Visibility.Visible;
			CevapKutusu.Visibility = Visibility.Visible;
			CevapVerildi = true;
		}
		
		private async void OyunuBaslat_Func()
		{
			string Operasyonlar = "";
			foreach (char Operasyon in Operasyonlar)
			{
				Operasyonlar += Operasyon;
			}
			while (true)
			{
				char Operasyon = Operasyonlar[RNG.Next(0, Operasyonlar.Length)];
				int IkinciSayi = (Operasyon == '+' || Operasyon == '-') ? RNG.Next(1, 100) : RNG.Next(1, 10);
				int BirinciSayi = (Operasyon == '+' || Operasyon == '-') ? RNG.Next(1, 100) : RNG.Next(1, 10) * IkinciSayi;
				Sonuc = Operasyon == '+' ? (BirinciSayi + IkinciSayi) : Operasyon == '-' ? (BirinciSayi - IkinciSayi) : Operasyon == '*' ? (BirinciSayi * IkinciSayi) : (BirinciSayi / IkinciSayi);
				
				SoruYazisi.Content = $"{BirinciSayi}{Operasyon}{IkinciSayi} işleminin sonucu nedir?";
				CevapKutusu_Border.Visibility = Visibility.Visible;
				CevapKutusu.Focus();

				do
				{
					await Task.Delay(TimeSpan.FromMilliseconds(0.5));
				} while (CevapVerildi != true);
				CevapVerildi = false;
			}
		}
		
		private void OyunuBaslat_Click(object Nesne, MouseButtonEventArgs OlayBilgisi)
		{
			Ayarlar.Visibility = Visibility.Hidden;
			CevapKutusu_Border.Visibility = Visibility.Hidden;
			Oyun.Visibility = Visibility.Visible;
			OyunuBaslat_Func();
		}
	}
}
