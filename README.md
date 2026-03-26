# KemDigSl

KemDigSl je Windows desktop aplikacija za lokalnu AI obradu i transformaciju slika. Korisnicko sucelje razvijeno je u C#/.NET WinForms okruzenju, dok se obrada izvrsava putem Python skripti, modela strojnog ucenja i vanjskih alata.

Ovaj repozitorij sluzi kao izvorni razvojni repozitorij. Za distribucijsku verziju aplikacije i gotove instalacijske artefakte koristi se zaseban instalacijski repozitorij.

## Repozitoriji

- Source code: <https://github.com/mateomustapic3/KemDigSl_Source>
- Installer/distribucija: <https://github.com/mateomustapic3/KemDigSl>

## Glavne funkcionalnosti

Glavni moduli aplikacije organizirani su po formama:

| Forma | Modul | Opis |
|---|---|---|
| `Form1` | Basic Transforms | Osnovne transformacije slike, ukljucujuci grayscale, blur, rotaciju, kontrast i zasicenje. |
| `Form2` | ESRGAN | Povecanje ili smanjenje rezolucije slike pomocu ESRGAN alata. |
| `Form3` | GFPGAN | Obnova lica na degradiranim ili starim fotografijama. |
| `Form4` | CodeFormer | Uklanjanje suma i restauracija lica uz podesavanje fidelity parametra. |
| `Form5` | DDColor | Automatska kolorizacija crno-bijelih slika. |
| `Form6` | AdaIN Style Transfer | Prijenos stila s referentne slike na ulaznu sliku. |
| `Form7` | YOLOv8 Detection | Detekcija objekata i prikaz bounding box rezultata. |
| `Form8` | Cartoonify | Stilizacija slike u anime ili cartoon stil pomocu AnimeGANv2 modela. |
| `Form9` | Film Restore | Uklanjanje grain i scratch artefakata uz LaMa/OpenCV fallback. |
| `Form10` | Object Removal | Uklanjanje objekata pomocu maske uz LaMa/OpenCV fallback. |
| `Form11` | AutoFix | Poluautomatizirani pipeline koji kombinira vise modula. |

## Tehnologije

- .NET 8 (`net8.0-windows`)
- Windows Forms
- Python runtime i pomocne skripte
- ESRGAN NCNN Vulkan
- GFPGAN
- CodeFormer
- DDColor
- AdaIN
- YOLOv8
- LaMa

## Arhitektura

- UI sloj: WinForms forme i navigacija kroz `MainMenuForm`
- Orkestracija: C# kod priprema ulaze, pokrece procese i ucitava rezultate
- Obrada: Python skripte i/ili nativni alati za pojedine AI module
- Upravljanje putanjama i resursima: `AppPaths.cs` pronalazi root aplikacije, Python runtime i povezane foldere

## Struktura projekta

Najvazniji dijelovi repozitorija:

- `Project.csproj` - .NET projekt
- `Project.sln` - solution datoteka
- `Program.cs` - ulazna tocka aplikacije
- `MainMenuForm.cs` - glavni izbornik i ucitavanje modula
- `WelcomeForm.cs` - pocetni prikaz aplikacije
- `Form1.cs` do `Form11.cs` - funkcionalni moduli
- `AppPaths.cs` - rezolucija putanja do runtimea i asseta
- `CrashLogger.cs` - zapisivanje neocekivanih gresaka
- `python/` - pomocne Python skripte i wrapperi
- `ESRGAN/`, `CODEFORMER/`, `DDCOLOR/`, `GFPGAN/`, `DETECTION/`, `STYLE_TRANSFER/`, `BCKG_REMOVAL/`, `CARTOONIFY/` - modeli, skripte i resursi po modulima
- `KemDigSlInstaller.Quick.iss` - Inno Setup skripta za quick installer
- `tools/build_quick_installer.ps1` - build quick installera
- `tools/prepare_quick_installer_payload.ps1` - priprema payloada za quick installer

## Pokretanje iz izvornog koda

Preporuceno razvojno okruzenje:

1. Instalirati .NET 8 SDK
2. Instalirati Visual Studio 2022 ili noviji s WinForms podrskom
3. Otvoriti `Project.sln`

Build:

```powershell
dotnet build .\Project.sln -c Debug
```

Pokretanje:

```powershell
dotnet run --project .\Project.csproj
```

Napomena: pojedini moduli ovise o modelima, Python runtimeu i dodatnim resursima. Ako dio asseta nedostaje, odgovarajuci modul nece biti funkcionalan dok se resursi ne dodaju.

## Build i distribucija

Brzi installer:

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\build_quick_installer.ps1 -Configuration Release -Runtime win-x64 -SelfContained
```

Glavni izlazni artefakti generiraju se u `dist/installer`.

## Napomena o distribuciji

Preporuceni model rada:

- ovaj repozitorij koristi se za razvoj izvornog koda
- instalacijski repozitorij koristi se za objavu gotovih buildova i distribucijskih artefakata

Takav pristup odrzava source repozitorij preglednim i prikladnim za razvoj, dok se build i distribucija drze odvojeno.
