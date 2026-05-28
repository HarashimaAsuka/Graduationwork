const int pressurePin = A0;
const int lightPin1 = A1;
const int lightPin2 = A2;
const int irPin = 6;
const int buttonPin = 7;

void setup()
{
  Serial.begin(9600);

  pinMode(irPin, INPUT);
  pinMode(buttonPin, INPUT_PULLUP);
}

void loop()
{
  int i = analogRead( 5 );
  float f = i * 5.0 / 1023.0;
  Serial.println( f );
  delay( 1000 ); 
 
  int pressure = analogRead(pressurePin);
  int light1 = analogRead(lightPin1);
  int light2 = analogRead(lightPin2);
  int ir = digitalRead(irPin);
  int button = digitalRead(buttonPin);

  // 圧力センサー
  if (pressure > 1000)
  {
    Serial.println("PRESS");
  }

  // 照度センサー1
  if (light1 > 300)
  {
    Serial.println("LIGHT1");
  }

  // 照度センサー2
  if (light2 > 300)
  {
    Serial.println("LIGHT2");
  }

  // 赤外線センサー
  if (ir == HIGH)
  {
    Serial.println("HUMAN");
  }

  //ボタン
  if(button == LOW)
  {
    Serial.println("PUSH");
  }

  delay(100);
}