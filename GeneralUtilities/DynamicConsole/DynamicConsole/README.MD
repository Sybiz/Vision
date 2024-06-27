## Dynamic Console
This project shows simply how to build a project that can dynamically load assemblies for Sybiz Vision

## Connection Strings and User
Make sure you setup connection string and user variables at the top of Program.cs

## Reference Mode
Used to test application or build application
You will need to add all references set in the project to use from Sybiz Vision

then in the Vision Service class at the top should be set as so

#if DEBUG
// WHEN THIS IS UN COMMENTED REMOVE ALL VISION REFERENCES FROM PROJECT AND CLEAN
//#define DYNAMIC
#else
#define DYNAMIC
#endif

## Dynamic Mode in Debug mode 
You will need to remove all references relating to Sybiz Vision and clean project

then in the Vision Service class at the top should be set as so

#if DEBUG
// WHEN THIS IS UN COMMENTED REMOVE ALL VISION REFERENCES FROM PROJECT AND CLEAN
#define DYNAMIC
#else
#define DYNAMIC
#endif

## Release Mode
You will need to remove all references relating to Sybiz Vision and clean project

then in the Vision Service class at the top should be set as either as it does not matter

#if DEBUG
// WHEN THIS IS UN COMMENTED REMOVE ALL VISION REFERENCES FROM PROJECT AND CLEAN
//#define DYNAMIC
#else
#define DYNAMIC
#endif

#if DEBUG
// WHEN THIS IS UN COMMENTED REMOVE ALL VISION REFERENCES FROM PROJECT AND CLEAN
#define DYNAMIC
#else
#define DYNAMIC
#endif
