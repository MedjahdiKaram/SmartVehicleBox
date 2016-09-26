#include <NewPing.h>

#define TRIGGER_PIN  8  // Arduino pin tied to trigger pin on the ultrasonic sensor.
#define ECHO_PIN     10  // Arduino pin tied to echo pin on the ultrasonic sensor.
#define MAX_DISTANCE 200 // Maximum distance we want to ping for (in centimeters). Maximum sensor distance is rated at 400-500cm.
int led = 13; 
int buzzerPin = 11; 
NewPing sonar(TRIGGER_PIN, ECHO_PIN, MAX_DISTANCE); // NewPing setup of pins and maximum distance.

void setup()
{
      Serial.begin(9600); 
      Serial.flush();
      pinMode(led, OUTPUT);   
      pinMode(buzzerPin, OUTPUT);         
} 

bool Contains(String s, String search) {
    int max = s.length() - search.length();

    for (int i = 0; i <= max; i++) 
    {
        if (s.substring(i) == search) return true; // or i
    }
    return false; //or -1
}
void beep(int delayms){
  analogWrite(buzzerPin, 20);      // Almost any value can be used except 0 and 255
                           // experiment to get the best tone
  delay(delayms);          // wait for a delayms ms
  analogWrite(buzzerPin, 0);       // 0 turns it off
  delay(delayms);          // wait for a delayms ms   
}  

 void loop()
 {   
  int sensorValue = analogRead(A0);
  float voltage= sensorValue * (5.0 / 1023.0);
  delay(150);
  digitalWrite(led, LOW);
   int dist=sonar.ping_cm();
    delay (250);
   
    int dist2=sonar.ping_cm();
    delay (250);

//Serial.println(dist);
    //Serial.println(dist2);
    //Serial.println("________________");

    if (sqrt(pow(dist-dist2,2))>10)
      {

        beep(1000);
      }
Serial.print("distance ===> ");
Serial.println(dist);
    Serial.print("distance2 ===> ");
Serial.println(dist2);
  if (voltage<3 || voltage>3.9)  
   {
      Serial.println("! hmida shock !");
    }
    String command = "";   
    while (Serial.available() > 0)
    {
        command +=  Serial.readString();      
        delay(5);
    }

    if (Contains(command, "stop")==1)
    {        
        digitalWrite(led, HIGH); 
        delay(600000);
    }
    else   //if (Contains(command, "off")==1)
    {
         digitalWrite(led, LOW);
    }   
 }
