Scenario
--------
Vidados sends out lots of HTML newsletters.  These feature different trips and hosts (the people that supply the holidays).

This web app generates newsletters.  

The task
--------
There are 2 elements to this task.  You should complete at least one - bonus points for both:

1. Allow the order and quantity of trips and hosts to be specified.  For example we might decide we want newsletters to have 3 trips followed by 2 hosts followed by 3 trips (TTTHHTTT).  

It should be possible to
 a) enter this setting through the web interface
 b) have this setting persisted so that all generated emails use this setting until it is changed

 2. Make the allocation of trips and hosts *fair*.

 Hosts get upset if their trips are featured less than other hosts'.  Therefore we want it so that when a newsletter is generated, preference is given to those trips and hosts that have thus far featured on fewer newsletters.

 How to get going
 ----------------
 1. clone the repository
 2. open in Visual Studio 2015 (the community edition is free)
 3. build and run the app by pressing F5
 4. click on the Newsletters link to see the generated newsletters.  Initially there will not be any.
 5. you can view a newsletter sample with randomly created trips and hosts
 6. back on the home page click on "Create entities" to create some test trips and hosts
 7. now you can generate newsletters
 8. there is also a delete button to delete all the newsletters you've created

 What we're looking for
 ----------------------
 There's no right way to achieve this.  Please feel free to ask any questions.  Unit tests would be great so that we can easily assess how well the code works, but mainly you should be able to justify whatever decisions you make - with respect to any stated assumptions.

 How to submit
 -------------
 Ideally a pull request but if you're not familiar with this then please zip up and send the repo back.