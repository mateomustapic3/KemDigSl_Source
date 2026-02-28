# KemDigSl (WindowsFormsApp)

KemDigSl je desktop aplikacija za lokalnu AI obradu slika na Windows platformi.  
GUI je napravljen u C#/.NET WinForms, a obrada se pokrece kroz Python skripte i vanjske alate.

Ovaj repozitorij je namijenjen prvenstveno za razvoj i source kod.

## Instalacijski repozitorij

Za gotovu instalaciju aplikacije koristi ovaj repozitorij:  
<https://github.com/mateomustapic3/KemDigSl>

## Sto aplikacija radi

Glavni moduli (po formama):

| Forma | Modul | Opis |
|---|---|---|
| Form1 | Basic Transforms | Osnovne transformacije slike (npr. grayscale, blur, rotate, contrast, saturation). |
| Form2 | ESRGAN | Upscale/Downscale slika (x2/x4/x8/x16) preko `realesrgan-ncnn-vulkan.exe`. |
| Form3 | GFPGAN | Obnova lica na degradiranim fotografijama. |
| Form4 | CodeFormer | Denoise/restore lica i detalja uz podesavanje fidelity parametra. |
| Form5 | DDColor | Automatska kolorizacija crno-bijelih slika. |
| Form6 | AdaIN Style Transfer | Prijenos stila sa style slike na ulaznu sliku. |
| Form7 | YOLOv8 Detection | Detekcija objekata i prikaz bounding box rezultata. |
| Form8 | Cartoonify | Anime/cartoon stilizacija slike (AnimeGANv2). |
| Form9 | Film Restore | Uklanjanje grain/scratch artefakata (LaMa/OpenCV fallback). |
| Form10 | Object Removal | Uklanjanje objekata pomocu maske (LaMa/OpenCV fallback). |
| Form11 | AutoFix | Polu-automatiziran pipeline koji kombinuje vise modula. |

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

## Arhitektura (ukratko)

- UI sloj: WinForms forme i navigacija kroz `MainMenuForm`.
- Orkestracija: C# kod priprema ulaze, pokrece procese i cita izlaze.
- Obrada: Python skripte i/ili native alati (npr. ESRGAN exe).
- Asset rezolucija: `AppPaths.cs` pronalazi app root, Python runtime i potrebne foldere.

## Struktura projekta

Najvazniji dijelovi:

- `Project.csproj` - .NET projekt
- `Program.cs` - ulazna tacka aplikacije
- `MainMenuForm.cs` - glavni meni i otvaranje modula
- `Form1.cs` ... `Form11.cs` - funkcionalni moduli
- `AppPaths.cs` - trazenje root putanja i python.exe
- `CrashLogger.cs` - logiranje neocekivanih gresaka
- `python/` - helper skripte i GFPGAN wrapper
- `ESRGAN/`, `CODEFORMER/`, `DDCOLOR/`, `GFPGAN/`, `DETECTION/`, `STYLE_TRANSFER/`, `BCKG_REMOVAL/`, `CARTOONIFY/` - modeli, skripte i asseti po modulima
- `tools/package_portable.ps1` - skripta za portable paket

## Pokretanje iz source koda

Napomena: ovaj source repo moze biti bez punog runtime/model seta.  
Ako zelis "radi odmah" setup, koristi instalacijski repozitorij:
<https://github.com/mateomustapic3/KemDigSl>

Minimalni koraci za development:

1. Instaliraj .NET 8 SDK i Visual Studio 2022 (ili noviji) sa WinForms workloadom.
2. Otvori `Project.sln`.
3. Build:

```powershell
dotnet build .\Project.csproj -c Debug
```

4. Run:

```powershell
dotnet run --project .\Project.csproj
```

Ako nedostaju modeli/runtime folderi, pojedini moduli nece raditi dok ne dodas potrebne assete.

## Build i portable paket

Portable paket (Release, win-x64):

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\package_portable.ps1 -Configuration Release -Runtime win-x64
```

Skripta ce napraviti izlaz u `dist/portable/app`.

## Repozitorij strategija

Preporuceno:

- ovaj repo: source kod i razvoj
- instalacijski repo: gotove build/installer artefakte

Na taj nacin source ostaje cist, manji i laksi za odrzavanje, a distribucija je odvojena.

## Licenciranje i trece strane

Projekt ukljucuje vendorizirane komponente trecih strana (npr. GFPGAN, CodeFormer, LaMa, AdaIN, AnimeGANv2, DDColor).  
Svaki podprojekt moze imati vlastitu licencu i uslove koristenja, pa provjeri licence u njihovim podfolderima prije distribucije.
