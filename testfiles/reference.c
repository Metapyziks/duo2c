#include <stdio.h>

int i; double pi;

int main() {
    i = 1;
    pi = 4;

    while (i < 1000000) {
        pi = pi - 4.0 / (i * 4 - 1) + 4.0 / (i * 4 + 1);
        i = i + 1;
    }
    printf("%f", pi); printf("\n");
    return 0;
}
