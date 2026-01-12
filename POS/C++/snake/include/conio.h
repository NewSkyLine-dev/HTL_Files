#ifndef CONIO_H
#define CONIO_H

#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <sys/select.h>
#include <termios.h>
#include <sys/ioctl.h>
#include <stdio.h>

void reset_terminal_mode();
void set_conio_terminal_mode();
int kbhit();
int getch();
int get_terminal_width();
int get_terminal_height();

#endif