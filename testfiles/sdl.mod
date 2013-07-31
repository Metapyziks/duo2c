MODULE SDL;
    IMPORT Out;

    CONST
        InitNone  = {   };
        InitAudio = { 1 };
        InitVideo = { 2 };
        InitCDROM = { 3 };
        InitTimer = { 4 };

    TYPE
        InitPackage : SET;

    PROCEDURE Init* (packages : InitPackage) : INTEGER;
    EXTERNAL SDL_Init FROM "SDL2.dll";
    
    PROCEDURE GetError* () : ARRAY OF CHAR;
    EXTERNAL SDL_GetError FROM "SDL2.dll";

    PROCEDURE Quit*;
    EXTERNAL SDL_Quit FROM "SDL2.dll";

BEGIN
    IF SDL.Init(SDL.InitAudio + SDL.InitVideo) < 0 THEN
        Out.String(SDL.GetError());
    ELSE        
        (* Do some things... *)
        SDL.Quit;
    END;
END SDL.
