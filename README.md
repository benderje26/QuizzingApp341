This is our app for CS 341. Here is our proposal:

Our idea stems from the idea proposed by Sabrina in the App Ideas list as well as our own experience using pollev.com in various CS courses. The idea is
to make a new quizzing app that can be used by instructors in class, or by students studying. The first main function would be creating quizzes, which can
be multiple choice questions or fill in the blank. Another main function will be conducting the quizzes which involves other users (or guests not logged
in) to scan a QR Code or enter a code to enter the quiz. A third function is storing history of past results, so instructors can see how students are 
performing. A fourth function is to be able to favorite quizzes to be able to retake them again.​


Sprint 2 Changes

Peter Skogman: I made the question pages for the quiz bind to the current questions of the quiz. Users can navigate between the questions with previous and next buttons, and a submit button at the end to return back to the home screen. 

Pachia: The box plot for the statistics screen is not an image anymore, it is actually created based on given data 😀 

Jason W: I did the navigation bar. The login screen will be shown when the user first launches the app. After the user correctly logs in, the userHome screen will show up, and the navigation bar will also show up at the bottom.

Jeremiah: I worked on the database mainly. Sadly, Supabase is having a bit of trouble and I am unable to get it to load, but hopefully this should be quickly resolved. I also overall did the model for the quiz question types.

Zafeer Rahim: I made the homescreen page. The changes were that now the buttons in homescreen has functionality. I also made a default code for quiz(12345) that takes the user to questions page.


Sprint 3 Changes

Peter Skogman: I contributed to the development of getting the quiz to function locally. For me, this entailed changing some of the models and adding/altering methods within the business layer. There also is now a pop up after the submit button is pressed to alert the user of how many questions they got correct. 

Zafeer: I attempted to load quizzes from the database, but unfortunately, I ran into issues that prevented it from working as expected. As a result, I decided to use hardcoded questions instead. 

Pachia: 

Jason W: I added more functionality to the create new quiz, and changed the original one to "My quiz", when the user clicks on the + sign, it brings the user to createNewQuiz screen, and if the user clicks on "+ new question", a pop-up will show and ask for the question types, and each question type with bring the user to corresponding screens.  

Jeremiah: 
