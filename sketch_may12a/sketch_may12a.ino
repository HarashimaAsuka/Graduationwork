const int pressurePin = A0;
const int lightPin1 = A1;
const int lightPin2 = A2;
const int trigPin = 2;//超音波センサ
const int echoPin = 3;//超音波センサ
const int irPin = 6;//赤外線センサ
const int buttonPin = 7;//ボタン

double duration = 0;
double distance = 0;

bool wasBright1 = false;
bool wasBright2 = false;

void setup()
{
  Serial.begin(9600);

  pinMode(irPin, INPUT);
  pinMode(buttonPin, INPUT_PULLUP);
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
}

void loop()
{
  // int i = analogRead( 5 );
  // float f = i * 5.0 / 1023.0;
  // Serial.println( f );
  // delay( 1000 ); 
 
  int pressure = analogRead(pressurePin);
  int light1 = analogRead(lightPin1);
  int light2 = analogRead(lightPin2);
  int ir = digitalRead(irPin);
  int button = digitalRead(buttonPin);

//超音波センサ
  // trig初期化
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);

  // 超音波発射
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);

  // 反射時間取得
  duration = pulseIn(echoPin, HIGH);

  // 距離計算
  distance = duration * 0.034 / 2;

  // 距離表示
  Serial.print("距離: ");
  Serial.print(distance);
  Serial.println(" cm");

  if(distance <= 10)
  {
    Serial.println("CLOSE");
  }


  // 圧力センサー
  if (pressure > 1000)
  {
    Serial.println("PRESS");
  }

  // 照度センサー1
  bool isBright1 = (light1 < 300);

  if (isBright1 && !wasBright1)
  {
      Serial.println("LIGHT1");
  }

  wasBright1 = isBright1;

  // 照度センサー2
  bool isBright2 = (light2 < 300);

  if (isBright2 && !wasBright2)
  {
      Serial.println("LIGHT2");
  }

  wasBright2 = isBright2;

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