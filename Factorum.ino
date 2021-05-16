/*
 Analog input reads an analog input on analog in 0, prints the value out.
 created 24 March 2006
 by Tom Igoe
 */
int temp = 23;
int tempPin = A0;
int setpoint = 50;
bool Heater1 = false;
int h1pin = 22;
bool Heater2 = false;
int h2pin = 24;
unsigned long lastMillis;
int setpointtolerance =3;
int fullpowermultiplier = 3;
int analogValue = 0;    // variable to hold the analog value
void setup() {
  // open the serial port at 9600 bps:
  Serial.begin(9600);
  pinMode(h1pin,OUTPUT);
  pinMode(h2pin, OUTPUT);
  lastMillis = millis();
}

void loop() {
  // read the analog input on pin 0:
  temp = 100 * (float) analogRead(tempPin)/1024;
  if (millis() - lastMillis >= 10*1000L)
  {
    
    lastMillis = millis(); 
    int dT = temp - setpoint;
  
    if (dT > setpointtolerance)
    {
      //We zitten boven ons setpoint, warmte elementen uit!!
      digitalWrite(h1pin, LOW);
      digitalWrite(h2pin, LOW);
      Heater1 = false;
      Heater2 = false; 
    }
    else if (dT < -fullpowermultiplier*setpointtolerance)
    {
      digitalWrite(h1pin, HIGH);
      digitalWrite(h2pin, HIGH);
      Heater1 = true;
      Heater2 = true; 
    }
    else if (dT < -1.5*setpointtolerance)
    {
      digitalWrite(h1pin, LOW);
      digitalWrite(h2pin, HIGH);
      Heater1 = false;
      Heater2 = true; 
    }
    else
    {
      digitalWrite(h1pin, HIGH);
      digitalWrite(h2pin, LOW);
      Heater1 = true;
      Heater2 = false;
    } 
   }

  String tosend = "";
  if (Serial.available() > 0)
  {
    String msg = Serial.readString();
    if (msg.substring(0,1) == "s")
    {
      int val = msg.substring(1).toInt();
      setpoint = val;
      tosend += 's';
      tosend += (String) setpoint;
      tosend += '.';
    }
    tosend += 't' + (String) temp;
    tosend += '.';
    tosend += 'h';
    tosend += (String) Heater1;
    tosend += '.';
    tosend += 'H';
    tosend += (String) Heater2;
    tosend += '.';
    Serial.println(tosend);
  }
  

}
