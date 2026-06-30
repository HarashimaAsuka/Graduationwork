const int lightPin1 = 9;//照度センサ
const int lightPin2 = 10;//照度センサ
const int lightPin3 = 11;//照度センサ
const int trigPin = 2;//超音波センサ
const int echoPin = 3;//超音波センサ

double duration = 0;
double distance = 0;

bool wasClose = false;

bool wasBright1 = false;
bool wasBright2 = false;
bool wasBright3 = false;

void setup()
{
  Serial.begin(9600);

  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);

  pinMode(lightPin1, INPUT);
  pinMode(lightPin2, INPUT);
  pinMode(lightPin3, INPUT);
}

void loop()
{
  // int i = analogRead( 5 );
  // float f = i * 5.0 / 1023.0;
  // Serial.println( f );
  // delay( 1000 ); 
 
  int light1 = digitalRead(lightPin1);
  int light2 = digitalRead(lightPin2);
  int light3 = digitalRead(lightPin3);

  //超音波センサ
  // trig初期化
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);

  // 超音波発射
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);

  // 反射時間取得
  duration = pulseIn(echoPin, HIGH, 30000);

  if (duration == 0)
  {
    // 測定失敗なので今回は無視
    delay(100);
    return;
  }

  // 距離計算
  distance = duration * 0.034 / 2;

  if (distance < 2 || distance > 400)
  {
    delay(100);
    return;
  }

  // 距離表示
  Serial.print("距離: ");
  Serial.print(distance);
  Serial.println(" cm");

  bool isClose = (distance <= 10);
  
  if(isClose && !wasClose)
  {
    Serial.println("CLOSE");
  }

  wasClose = isClose;

  // 照度センサー1
  bool isBright1 = (light1 == LOW);

  if (isBright1 && !wasBright1)
  {
      Serial.println("LIGHT1");
  }

  wasBright1 = isBright1;

  // 照度センサー2
  bool isBright2 = (light2 == LOW);

  if (isBright2 && !wasBright2)
  {
      Serial.println("LIGHT2");
  }

  wasBright2 = isBright2;

// 照度センサー3
  bool isBright3 = (light3 == LOW);

  if (isBright3 && !wasBright3)
  {
      Serial.println("LIGHT3");
  }

  wasBright3 = isBright3;
}