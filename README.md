# CryptoViewer
CryptoViewer is a desktop application designed for cryptocurrency enthusiasts and investors. It provides real-time data on cryptocurrency prices, market trends, and detailed coin information. The application leverages the CoinGecko API to fetch up-to-date data and presents it in an intuitive and user-friendly interface.

---


## Features
- Real-Time Data: Get the latest cryptocurrency prices, market caps, and trading volumes updated in real time.

- Detailed Coin Information: Access in-depth details about individual coins, including historical price charts and market trends.

- Search Functionality: Quickly find specific cryptocurrencies using the built-in search bar.

- Currency Converter: Convert between various cryptocurrencies and fiat currencies seamlessly.

- User-Friendly Interface: Enjoy a clean, intuitive design suitable for both beginners and seasoned investors.

- Localization: Switch between multiple languages to make the app accessible to a global audience.

--- 

## Installation

Follow these steps to set up CryptoViewer on your local machine:

1. Clone the Repository:
   -   Open your terminal and run the following command to download the project files:
   
   ```
   git clone https://github.com/yourusername/CryptoViewer.git
   ```
2. Navigate to the Project Directory:
    -   Move into the project folder:
   ```
   cd CryptoViewer
   ```
3. Restore Dependencies:
    -   Ensure you have the .NET SDK installed on your system.
    -   Restore the required packages by running:
```
dotnet restore
```
4. Build the Project:
    - Compile the application with:
```
dotnet build
```
5. Run the Application:
    - Launch CryptoViewer using:

```
dotnet run
```

---

# Usage
- Once CryptoViewer is up and running, here’s how you can use it:
- View the Main Dashboard: The home screen displays a list of top cryptocurrencies with their current prices, market caps, and trading volumes.

- Search for Coins: Use the search bar at the top to locate specific cryptocurrencies by name or ticker symbol.

- Convert Currencies: Navigate to the currency converter tool to exchange values between cryptocurrencies and fiat currencies like USD or EUR.

- Switch Languages: Adjust the app’s language settings via the preferences menu to suit your needs.

--- 

# Configuration
CryptoViewer uses configuration files to manage settings such as API keys and language preferences. Here’s how to set it up:
API Key Configuration:
- CryptoViewer relies on the CoinGecko API for data. Sign up at CoinGecko to get your free API key.

  -Open the appsettings.json file in the project root and add your API key like this:
```json
{
  "CoinGecko": {
    "ApiKey": "YOUR_API_KEY_HERE"
  }
}
```
- Language Settings:
    - The app supports multiple languages. To customize or add new translations, modify the resource files in the Resource folder.

--- 

## Contributing
I love contributions from the community! Here’s how you can help improve CryptoViewer:
1. Fork the Repository:
    - Click the "Fork" button on the top right of the GitHub repository page to create your own copy.

2. Create a New Branch:
    - Set up a branch for your changes:
```
git checkout -b feature/your-feature-name
```

3. Make Your Changes:
    - Add your feature, fix a bug, or improve the code as needed.

4. Commit Your Changes:
    - Save your work with a descriptive message:
```
git commit -m "Add your commit message here"
```
5. Push to Your Fork:
    - Upload your changes to your GitHub fork:
```
git push origin feature/your-feature-name
```
6. Create a Pull Request:
    - Visit the original repository on GitHub, click "New Pull Request," and submit your changes for review.

Please follow the project’s coding standards and include tests where applicable.

---

## License
This project is licensed under the MIT License. For more details, see the LICENSE file in the repository.

---

# Contact
Have questions or need support? Reach out to me:<br>
Email: zssjzss@gmail.com <br>
Telegram: @mr_bebrabaiden

---

# Thank you for checking out CryptoViewer!


























