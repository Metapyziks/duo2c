#include <stdio.h>
#include <windows.h>

typedef int (*SDL_Init_t)(void);
typedef char* (*SDL_GetError_t)(void);
typedef void (*SDL_Quit_t)(void);

HMODULE sdl;

SDL_Init_t SDL_Init;
SDL_GetError_t SDL_GetError;
SDL_Quit_t SDL_Quit;

int sdl_initialized = 0;

void cleanup() {
    if (sdl_initialized) {
        SDL_Quit();
    }

    if (sdl != NULL) {
        FreeLibrary(sdl);
    }
}

int main() {
    HMODULE sdl = LoadLibrary("SDL2.dll");
    if (sdl == NULL) {
        printf("Could not load library!\n");
        exit(1);
    }

    FARPROC init     = GetProcAddress(sdl, "SDL_Init");
    FARPROC getError = GetProcAddress(sdl, "SDL_GetError");
    FARPROC quit     = GetProcAddress(sdl, "SDL_Quit");

    if (init == NULL || getError == NULL || quit == NULL) {
        printf("Could not find procedure!\n");
        exit(1);
    }

    SDL_Init     = (SDL_Init_t) init;
    SDL_GetError = (SDL_GetError_t) getError;
    SDL_Quit     = (SDL_Quit_t) quit;

    if (SDL_Init() < 0) {
        printf("Unable to init SDL: %s\n", SDL_GetError());
        exit(1);
    }

    sdl_initialized = 1;

    printf("Great success!\n");

    exit(0);
}
