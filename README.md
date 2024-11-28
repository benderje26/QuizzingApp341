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

Jeremiah: I did a good chunk of the backend with the database and made it work with the UI code the rest of the team was working on. I also integrated the business logic to access information from the database, and I redid the API for the business logic so that it was more versatile.

Sprint 4 Changes

Zafeer: I have worked on the search screen that the user is navigated to from home screen, the search screen would show the quizzes available in the database and also the user should be able to search up any quiz title and find one to study.

Peter S: I worked mainly on the favorites tab this sprint. User can add/remove a favorite through a button on the search screen. Pink heart means it is a favorite, and black heart means it is not a favorite. Once the user navigates to the search screen they can study the quiz from the screen, or remove the favorite. Both adding and deleting are fully functional and refresh the data so user always sees the current data. Study doesn't currently work as due to the refactoring done during this sprint acivating a quiz is being redone and is incomplete as of now.

Jeremiah: I worked on a few things, but the main one was updating the database. I used the notes that mainly Pachia made to make the tables and the RLS policies. I also fixed some log-in and log-out bugs, although there may still be some that I haven't seen yet. Pachia and I made the guest log in button. I also just fixed some bugs that had to do with Supabase and async functions. Finally, Pachia and I got live quizzes to work (on the student's end, the teacher's end is next sprint). If you want to test it it is a little tricky, but there are instructions below.

Jason: I worked on the History screen to display the quizzes by pass in userId to Participants and ActiveQuiz tables, if datas shown on both tables, return that data and display the data on the History screen by date from most recent to lastest. Thinks to Pachia for helping me understand why before I can't retrieve the data back from SupaBase, because I keep skip login. 

Sprint 4 Tasks:

Favorites tab ✅

Fix bug with starting the quiz ✅ (The bug that was seen during the demo was not able to be replecated, and now irrelavent since Pachia's refactoring)

Create a users table and create the user data object in the project ✅ (Users table already existed and handled by supabase :) )

Created the ability to take live quizzes ✅, albeit with manually pretending to be the teacher on the database side ☐. Instructions are below if interested in testing.

Example User Name and Password:
benderje26@uwosh.edu
abc123_-

How to do live quiz in this sprint (if interested):
 1) Type in quiz id: 'letsquiz' and press start.
 2) On the SQL shell, type in:
	  update active_quizzes set current_question_no = 0 where access_code = 'letsquiz';
	and execute it, then on your device you should be able to submit an answer. Next, in the SQL shell type in:
	  update active_quizzes set current_question_no = 1 where access_code = 'letsquiz';
	and execute it, then on your device you should be able to submit an answer. Finally, in the SQL shell type in:
	  update active_quizzes set current_question_no = 1 where access_code = 'letsquiz';
	and execute it, then on your device you should be able to submit an answer.
 3) After doing this, you have to press back on the navigation twice (will be fixed later once the teacher side is implemented).
 4) Also note that due to unique constraints on the 'responses' table, you will be unable to do this a second time unless you delete all your rows on the 'responses' table. Feel free to delete all the rows on the table for convenience sake.

