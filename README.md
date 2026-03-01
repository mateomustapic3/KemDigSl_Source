# KemDigSl (WindowsFormsApp)

KemDigSl je desktop aplikacija za lokalnu AI obradu slika na Windows platformi.  
Grafičko sučelje izrađeno je u C#/.NET WinForms okruženju, dok se obrada pokreće putem Python skripti i vanjskih alata.

Ovaj repozitorij namijenjen je prvenstveno razvoju i radu sa izvornim kodom.

---

## Instalacijski repozitorij

Za gotovu instalacijsku verziju aplikacije koristi sljedeći repozitorij:  
<https://github.com/mateomustapic3/KemDigSl>

---

## Funkcionalnosti aplikacije

Glavni moduli (po formama):

| Forma | Modul | Opis |
|---|---|---|
| Form1 | Basic Transforms | Osnovne transformacije slike (npr. grayscale, blur, rotacija, kontrast, zasićenje). |
| Form2 | ESRGAN | Povećanje/smanjenje rezolucije slike (x2/x4/x8/x16) putem `realesrgan-ncnn-vulkan.exe`. |
| Form3 | GFPGAN | Obnova lica na degradiranim fotografijama. |
| Form4 | CodeFormer | Uklanjanje šuma i restauracija lica i detalja uz podešavanje fidelity parametra. |
| Form5 | DDColor | Automatska kolorizacija crno-bijelih slika. |
| Form6 | AdaIN Style Transfer | Prijenos stila sa referentne slike na ulaznu sliku. |
| Form7 | YOLOv8 Detection | Detekcija objekata i prikaz bounding box rezultata. |
| Form8 | Cartoonify | Stilizacija slike u anime/crtani stil (AnimeGANv2). |
| Form9 | Film Restore | Uklanjanje grain i scratch artefakata (LaMa/OpenCV fallback). |
| Form10 | Object Removal | Uklanjanje objekata pomoću maske (LaMa/OpenCV fallback). |
| Form11 | AutoFix | Poluautomatizirani pipeline koji kombinira više modula. |

---

## Tehnologije

- .NET 8 (`net8.0-windows`)
- Windows Forms
- Python skripte za AI obradu
- ESRGAN NCNN Vulkan
- GFPGAN
- CodeFormer
- DDColor
- AdaIN
- YOLOv8
- LaMa

---

## Arhitektura (ukratko)

- UI sloj: WinForms forme i navigacija putem `MainMenuForm`
- Orkestracija: C# kod priprema ulaze, pokreće procese i učitava izlaze
- Obrada: Python skripte i/ili nativni alati (npr. ESRGAN izvršna datoteka)
- Upravljanje resursima: `AppPaths.cs` pronalazi root direktorij aplikacije, Python runtime i potrebne mape

---

## Struktura projekta

Najvažniji dijelovi:

- `Project.csproj` — .NET projekt
- `Program.cs` — ulazna točka aplikacije
- `MainMenuForm.cs` — glavni izbornik i pokretanje modula
- `Form1.cs` ... `Form11.cs` — funkcionalni moduli
- `AppPaths.cs` — pronalaženje root putanja i python.exe
- `CrashLogger.cs` — zapisivanje neočekivanih pogrešaka
- `python/` — pomoćne skripte i GFPGAN wrapper
- `ESRGAN/`, `CODEFORMER/`, `DDCOLOR/`, `GFPGAN/`, `DETECTION/`, `STYLE_TRANSFER/`, `BCKG_REMOVAL/`, `CARTOONIFY/` — modeli, skripte i resursi po modulima
- `tools/package_portable.ps1` — skripta za izradu portable paketa

---

## Pokretanje iz izvornog koda

Napomena: ovaj repozitorij s izvornim kodom može ne sadržavati puni runtime i modele.  
Ako želiš potpuno funkcionalnu verziju "spremnu za korištenje", koristi instalacijski repozitorij:  
<https://github.com/mateomustapic3/KemDigSl>

Minimalni koraci za razvoj:

1. Instaliraj .NET 8 SDK i Visual Studio 2022 (ili noviji) s WinForms podrškom.
2. Otvori `Project.sln`.

Build:

```powershell
dotnet build .\Project.csproj -c Debug
```

Pokretanje:

```powershell
dotnet run --project .\Project.csproj
```

Ako nedostaju modeli ili runtime mape, pojedini moduli neće raditi dok se ne dodaju potrebni resursi.

---

## Build i portable paket

Izrada portable paketa (Release, win-x64):

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\package_portable.ps1 -Configuration Release -Runtime win-x64
```

Skripta generira izlaz u direktoriju `dist/portable/app`.

---

## Strategija repozitorija

Preporučeni pristup:

- ovaj repozitorij: izvorni kod i razvoj
- instalacijski repozitorij: gotovi build i instalacijski artefakti

Na taj način izvorni kod ostaje pregledan, manji i lakši za održavanje, dok je distribucija jasno odvojena.
