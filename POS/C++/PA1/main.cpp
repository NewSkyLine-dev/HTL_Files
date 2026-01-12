#include <iostream>
#include "pa.h"

using namespace std;

int main(int argc, char **argv)
{
    srand(time(NULL));

    int random_num_1 = random_card_value();
    string input = "";
    int anzahl = 0;
    print_card(random_num_1);
    while (true)
    {
        int random_num_2 = random_card_value(); 
        cout << "Ist die nächste Zahl größer oder kliner?";
        cin >> input;
        cout << endl;
        if (input == "g") {
            if (random_num_2 >= random_num_1) {
                anzahl++;
                random_num_1 = random_num_2;
                print_card(random_num_1);
            }
            else {
                break;
            }
        }
        else {
            if (random_num_2 < random_num_1) {
                anzahl++;
                random_num_1 = random_num_2;
                print_card(random_num_1);
            }
            else {
                break;
            }
        }
    }
    cout << endl;
    cout << "Du hast " << anzahl << " richtig geraten";

    return 0;
}