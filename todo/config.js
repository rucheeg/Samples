const config = {};

config.host = process.env.HOST || "https://sql-cosmos-ffocus.documents.azure.com:443/";
config.authKey =
  process.env.AUTH_KEY || "wjY3pPdKr2HCrhHxF26zdIgo2Kf5LjSOmEwCxcVE3b1NhkGC8b6ThKGQmlv8yr8cStNngnvHjwrgs9XF25EInA==";
config.databaseId = "Tasks";
config.containerId = "Items";

if (config.host.includes("https://localhost:")) {
  console.log("Local environment detected");
  console.log("WARNING: Disabled checking of self-signed certs. Do not have this code in production.");
  process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
  console.log(`Go to http://localhost:${process.env.PORT || '3000'} to try the sample.`);
}

module.exports = config;