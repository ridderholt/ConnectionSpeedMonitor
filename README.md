# SpeedDataVisualizer

## Background

I live in the countryside of sweden, my only option for a decent internet connection is via the mobile network (4G). I had some connectivity issues and when I tried to complain to my ISP they just to me the usual:

- Have you restarted the router?
- Have you factory reseted your roter?
- Make sure you have place the 4G reciever in an optimal place

So in order to gather evidence in how poorly their services are working I thought I would start to measure my speed Up, Down and also the Latency and to display this in some way.

## Measuring

Since there are many sites and services providing indenpendet measuring of internet speeds I started looking around for a simple way of automating the measuring and found [Bredbandskollen CLI](http://www.bredbandskollen.se/om/mer-om-bbk/bredbandskollen-cli/). This was exactly what I needed. After checking the documentation I could easly setup a scheduled task on my computer that ran the CLI three times a day and save the output to a file.

```
bbk_cli.exe --out=C:\somefolder\result.txt
```

## Visualize the results

Once I had gathered data for some day I started to write this small CLI to parse the output of Bredbandskollens CLI and visualize it in a nice chart.

The parsing i quite straight forward, it read the file line by line collecting dates, upload speed, download speed and latency. It then uses [XPlot](https://fslab.org/XPlot/) to generate a chart as HTML.

# Usage

The CLI takes two arguments:

1. The path to the file where the output of Bredbandskollen CLI is stored
2. The path where you want to resulting HTML file to be stored

```
SpeedDataVisualizer.exe C:/somefolder/result.txt C:/somefolder/chart.html
```
