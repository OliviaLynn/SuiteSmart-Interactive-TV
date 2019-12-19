# SuiteSmart Interactive TV
<img src="https://img.shields.io/badge/unity-2017.1.5f1-blue" /> <img src="https://img.shields.io/badge/platform-androidTV-blue" /> <img src="https://img.shields.io/badge/maintained%3F-no-red" /> <img src="https://img.shields.io/github/issues/OliviaLynn/SuiteSmart-Interactive-TV" />

An interactive smart TV home screen for SuiteSmart, a smart hotel company. Built in Unity for Android TV.

<img src="https://i.imgur.com/69AnOSC.png" />

## Features

### A Unique Experience
Displays personalized information based on nearby Bluetooth beacons. A query to our GraphQL database matches each identified Bluetooth address with individual user data.

### Real-Time Localized Weather
API calls to [OpenWeatherMap](https://openweathermap.org/api) give us the information that populates our weather panel to give guests a convenient snapshot of current and forecasted weather.

### Stay in the Loop
An regularly refreshed feed of events taken from Ticketmaster's [extensive API](https://developer.ticketmaster.com/products-and-docs/apis/discovery-api/v2/) lets guests stay updated on what's going on in their host city.

### Center Stage
The main panel allows all sorts of focuses: allowing the user to navigate the web without having to leave the app, letting the user flip through options on Netflix before launching a fullscreen viewing experience, or giving our advertising parterners a platform to give our guests unobtrusive recommendations for food and activities around Pittsburgh.

## Getting Started

### Prerequisites

## UniWebView
We use the [UniWebView](https://docs.uniwebview.com/) Unity plugin for the main panel, to allow it to display any webpage we specify. Make sure you have a copy of that in your `Plugins` folder.

## SimpleJSON
We use the [SimpleJSON](https://wiki.unity3d.com/index.php/SimpleJSON) Unity plugin to serialize JSON data. This is free and small so it's already in the `Plugins` folder.
