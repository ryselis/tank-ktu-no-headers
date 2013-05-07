#include <SPI.h>
#include <Ethernet.h>
//#include <SdFat.h>
//#include <SdFatUtil.h>
//#include <SD.h>

#define onPin             A0
#define mainGunLeft       5
#define mainGunRight      6
#define machineGunPin     A1
#define fireSimulationPin A2
#define mainGunUpDown     7
#define mainGunFirePin    3
#define digitalPotCS      9
#define drivePotChannel   1
#define rotatePotChannel  0
#define BUFSIZE           255
#define tankVersion       "1.1"

byte mac[] = { 0x90, 0xA2, 0xDA, 0x0D, 0x37, 0xE2 };
//byte ip[] = { 10,3,1,202 };
byte ip[] = { 192,168,42,202 };
unsigned long lastMessageTime = -1;

//Sd2Card card;
//SdVolume volume;
//SdFile root;
//SdFile file;

EthernetServer server(80);

void digitalPotWrite(int address, int value)
{
  digitalWrite(digitalPotCS, LOW);

  SPI.transfer(address);
  SPI.transfer(value);

  digitalWrite(digitalPotCS, HIGH); 
}

void initButton(int button)
{
  pinMode(button, OUTPUT);
  digitalWrite(button, HIGH);
}

void initSD()
{
/*  pinMode(10, OUTPUT);                       // set the SS pin as an output (necessary!)
  digitalWrite(10, HIGH);                    // but turn off the W5100 chip!
 
  if (!card.init(SPI_HALF_SPEED, 4)) Serial.println("card.init failed!");
 
  // initialize a FAT volume
  if (!volume.init(&card)) Serial.println("vol.init failed!");
 
  Serial.println("Volume is FAT");
  Serial.println(volume.fatType(),DEC);
  Serial.println();
 
  if (!root.openRoot(&volume)) Serial.println("openRoot failed");
 
  // list file in root with date and size
  Serial.println("Files found in root:");
  root.ls(LS_DATE | LS_SIZE);
  Serial.println();
 
  // Recursive list of all directories
  Serial.println("Files found in all dirs:");
  root.ls(LS_R);
 
  Serial.println();
  Serial.println("Done");  */
}

void setup()
{
  Serial.begin(115200);

  initButton(onPin);
  initButton(mainGunLeft);
  initButton(mainGunRight);
  initButton(machineGunPin);
  initButton(fireSimulationPin);
  initButton(mainGunUpDown);
  initButton(mainGunFirePin);
  
  pinMode(digitalPotCS, OUTPUT);
  digitalWrite(digitalPotCS, HIGH); 
  SPI.begin();
  
  digitalPotWrite(drivePotChannel, 127);
  digitalPotWrite(rotatePotChannel, 127);
  
  initSD();

  Ethernet.begin(mac, ip);
  
  server.begin();
  
  resetAll();
  Serial.println("Serveris startavo");
}

boolean pressButton(char* url, const char* command, int len, int pin, boolean state)
{
  if (strncmp(url, command, len) == 0)
  {
    digitalWrite(pin, state);

    return true;
  }
  else
  {
    return false;
  }
}

void sendSuccess(EthernetClient& client, char* url)
{
  Serial.println(strcat("Successs: ",url));
  client.println("HTTP/1.1 200 OK");
  client.println("Connnection: close");
  client.println("Content-Type: text/html");
  client.println();
  client.println(url);
}

boolean processButtons(char* urlString, EthernetClient& client)
{
  if (pressButton(urlString, "tank/on", 7, onPin, LOW)){ sendSuccess(client, urlString); return true; }
  if (pressButton(urlString, "tank/off", 8, onPin, HIGH)){ sendSuccess(client, urlString); return true; }
  if (pressButton(urlString, "turret/left/on", 14, mainGunLeft, LOW)){ sendSuccess(client, urlString); return true; }
  if (pressButton(urlString, "turret/left/off", 15, mainGunLeft, HIGH)){ sendSuccess(client, urlString); return true; }
  if (pressButton(urlString, "turret/right/on", 15, mainGunRight, LOW)){ sendSuccess(client, urlString); return true; }
  if (pressButton(urlString, "turret/right/off", 16,  mainGunRight, HIGH)){ sendSuccess(client, urlString); return true; }
  if (pressButton(urlString, "machine_gun/fire/on", 19, machineGunPin, LOW)){ sendSuccess(client, urlString); return true; }
  if (pressButton(urlString, "machine_gun/fire/off", 20, machineGunPin, HIGH)){ sendSuccess(client, urlString); return true; }
  if (pressButton(urlString, "main_gun/move/on", 16, mainGunUpDown, LOW)){ sendSuccess(client, urlString); return true; }
  if (pressButton(urlString, "main_gun/move/off", 17, mainGunUpDown, HIGH)){ sendSuccess(client, urlString); return true; }
  if (pressButton(urlString, "main_gun/fire_simulate/on", 25, fireSimulationPin, LOW)){ sendSuccess(client, urlString); return true; }
  if (pressButton(urlString, "main_gun/fire_simulate/off", 26,  fireSimulationPin, HIGH)){ sendSuccess(client, urlString); return true; }
  if (pressButton(urlString, "main_gun/fire/on", 16, mainGunFirePin, LOW)){ sendSuccess(client, urlString); return true; }
  if (pressButton(urlString, "main_gun/fire/off", 17, mainGunFirePin, HIGH)){ sendSuccess(client, urlString); return true; }

  return false;
}

boolean processThumbStick(char* urlString, EthernetClient& client)
{
  if (strncmp(urlString, "go/", 3) == 0)
  {
    {
      char* valueX = &urlString[3];
      valueX[3] = 0;
      int parsedXvalue = atoi(valueX);
      digitalPotWrite(drivePotChannel, parsedXvalue);
    }
    
    {
      char* valueY = &urlString[7];
      valueY[3] = 0;
      int parsedYvalue = atoi(valueY);
      digitalPotWrite(rotatePotChannel, parsedYvalue);
    }

    sendSuccess(client, urlString);
    
    return true;
  }
  
  return false;
}

boolean processControl(char* urlString, EthernetClient& client)
{
  if (strncmp(urlString, "move/", 5) == 0)
  {
    char* value = &urlString[5];
    int parsedValue = atoi(value);
    // digitalWrite(onPin, HIGH);
    digitalPotWrite(drivePotChannel, parsedValue);
    sendSuccess(client, urlString);
    // Serial.println("Move: " + parsedValue);

    return true;
  }

  if (strncmp(urlString, "rotate/", 7) == 0)
  {
    char* value = &urlString[7];
    int parsedValue = atoi(value);
    // digitalWrite(onPin, HIGH);
    digitalPotWrite(rotatePotChannel, parsedValue);
    sendSuccess(client, urlString);
    // Serial.println("Rotate: " + parsedValue);
    
    return true;
  }
  
  return false;
}

boolean processFiles(char* urlString, EthernetClient& client)
{
  /*
  char* fileName = urlString;
 
  (strstr(clientline, " HTTP"))[0] = 0;
 
  if (false == file.open(&root, fileName, O_READ))
  {
    client.println("HTTP/1.1 404 Not Found");
    client.println("Content-Type: text/html");
    client.println();
    client.println("<h2>File Not Found!</h2>");
    client.println(fileName);
    
    return true;
  }
  else
  {
    client.println("HTTP/1.1 200 OK");
            
    char* ext = fileName + strlen(fileName) - 3;
            
    if (strncmp("png", ext, 3) == 0)
    {
      client.println("Content-Type: image/png");
    }
    else
    {
      //  client.println("Content-Type: text/plain");
    }
            
    client.println();
 
    int16_t c;

    while ((c = file.read()) >= 0)
    {
      client.print((char)c);
    }

    file.close();
    
    return true;
  }
  */
  return false;
}

void processCommands(char* urlString, EthernetClient& client)
{
  if (true == processThumbStick(urlString, client))
  {
    return;
  }

  if (true == processControl(urlString, client))
  {
    return;
  }

  if (true == processButtons(urlString, client))
  {
    return;
  }
  
  if (true == processFiles(urlString, client))
  {
    return;
  }

  client.println("HTTP/1.1 404 Not Found");
  client.println("Content-Type: text/html");
  client.println("Connnection: close");
  client.println();
}

void resetOnIdle()
{
  if (millis() - lastMessageTime > 500)
  {
    digitalPotWrite(drivePotChannel, 127);
    digitalPotWrite(rotatePotChannel, 127);
  
    digitalWrite(onPin, HIGH);
    digitalWrite(mainGunLeft, HIGH);
    digitalWrite(mainGunRight, HIGH);
    digitalWrite(machineGunPin, HIGH);
    digitalWrite(fireSimulationPin, HIGH);
    digitalWrite(mainGunUpDown, HIGH);
    digitalWrite(mainGunFirePin, HIGH);    
  }  
}

void resetAll()
{
  if (millis() - lastMessageTime > 500)
  {
    digitalPotWrite(drivePotChannel, 127);
    digitalPotWrite(rotatePotChannel, 127);
  
    digitalWrite(onPin, HIGH);
    digitalWrite(mainGunLeft, HIGH);
    digitalWrite(mainGunRight, HIGH);
    digitalWrite(machineGunPin, HIGH);
    digitalWrite(fireSimulationPin, HIGH);
    digitalWrite(mainGunUpDown, HIGH);
    digitalWrite(mainGunFirePin, HIGH);    
  }  
}

void sendCORS(EthernetClient& client)
{
  //int headerStart = strstr(clientheaders, "Access-Control-Allow-Headers:");
  Serial.println("Sending cors");
  
  client.println("HTTP/1.1 200 OK");
  client.println("Access-Control-Allow-Origin: *");
  client.println("Access-Control-Allow-Methods: POST, GET, OPTIONS");
  client.println("Access-Control-Allow-Headers: X-Prototype-Version, X-Requested-With");
}

void loop()
{
  char clientline[BUFSIZE] = { 0 };
  char clientheaders[BUFSIZE] = { 0 };
  int index = 0;
  int index2 = 0;

  EthernetClient client = server.available();

  if (client)
  {
    index = 0;

    while (client.connected())
    {
      boolean readURL = false;
      if (client.available())
      {
        char c = client.read();

        if(!readURL)
        {
          if (c != '\n' && c != '\r' && index < BUFSIZE)
          {
            clientline[index++] = c;
            continue;
          }
        }  
        lastMessageTime = millis();

        client.flush();

        char* urlString = &clientline[5];

        Serial.println(clientline);
        //if (strncmp(clientline, "OPTI", 4) == 0) sendCORS(client);
        //else 
        Serial.println(tankVersion);
        processCommands(urlString, client);

        break;
      }
    }

    delay(10);

    client.stop();
  }
  
 // resetOnIdle();
}

