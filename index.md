Welcome to the page dedicated to the AOS program. First, let's ask ourselves what AOS is. 
This is a tool for generating Ronchi simulation images and then comparing them to the actual Ronchi images of the telescope under test. 
The main purpose of the analysis is to determine the amount of spherical aberration of the system and the occurrence of turned edges and zones of the telescope optics (mirror or lens).

AOS is a Windows application. For its correct installation and operation, .net version 6 is required, which, if necessary, is installed automatically together with AOS.

#### 2023.11.09 - AOS version 0.9.1 is now available. The program takes into account the definition of turned edges and zones. Wavefront error is calculated from raytracing, with a Airy disk hitting by light rays critterion.

[AOS v0.9.1 Download link](https://drive.google.com/file/d/1KXSklTsz-Npuy4GlbtK8EwA3wZMJM1E6/view?usp=sharing)

#### 2023.04.13 - AOS version 0.8.1 is now available.

[AOS v0.8.1 Download link](https://drive.google.com/file/d/1RsgUUwlxHL4G3dZE_JKWARTOY2_IveeM/view?usp=sharing)

If you feel the need to support the development of this interesting tool, you can support the author with any donation via [PayPal](https://paypal.me/mczarnackiAOS?country.x=PL&locale.x=pl_PL).
The donation is not obligatory and AOS is free.

## How to use AOS

With the help of the images below, I demonstrate how to use AOS. Let's start.

![Main window](/img/main_window.png)

Color explanation:
```
Green - optic parametres e.g. aperture, focal length, conic constant of mirror, Ronchi grating parameters - lines per mm, wavelength.
Blue - type of test performed (Ronchi or knife edge, at focus or at radius of mirror curvature).
Red - calculation start button.
Orange - zones definition window.
Brown - wavefront analysis start button.
Purple - wavefront analysis results (p-v, rms, Strehl coefficient, Airy disk diameter, and best focus).
```

![Main window](/img/main_window_2.png)

Color explanation:
```
Red - simulated ronchigram image.
Blue - photo from real optical test.
Orange - blink button for vizual differeces comparison.
Green - sliders for blending two images (shown later).
```

### How to make a Ronchi test simulation with AOS.

Let's look at the pictures below.
First, you have to determine optics and Ronchi grating parametres (diameter/aperture of objective/mirror, focal length, lines per mm usually 4 or more lines per mm).
Next, you have to chose where the Ronchi grating is located (relative to focal point or center of curvature).
Choose test method - Ronchi test or knife edge and choose wavelength. After that click on the "Claculate" button.

Here is an exaple for concave mirror (Newtonian telescope) with 150mm aperture and 1200mm of focal length.
The First image shows parabolic mirror in a Ronchi test in autocollimation mode.
The second image shows spherical mirror in the same test. In both cases Ronchi grating is located 5mm after focal point.

![Test](/img/test_1.png)

The next image shows additional zones window. Modelled optics is characterised with hudge turned edge on the last 15% of its diameter.

![Test turned edges](/img/test_2_turned_edge.png)

For decipher data from real Ronchi test, we can load test image. And next, we can choose experimental parameters to calculate ronchigram as similar as to
experminetal one. For such symulation AOS can estimate a wevefront error as below.

![Refractor test](/img/test_2_70_430_refractor.png)
