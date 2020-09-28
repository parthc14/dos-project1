# Lucas-Square-Pyramid
The project is based on the interesting problem of elliptic curve theory , the problem of finding the perfect squares that are sums of consecutive squares. 
Course: Distributed Operating Systems [COP5612] - Fall 2020
Language: F#
Project 1: Lucas' Square Pyramid


Project Members
Parth Paresh Chitroda UFID- 5189-1737
Saheel Ravindra Sawant UFID- 1164-7923


CONTENTS OF THIS FILE
1.Introduction
2.Requirements
3.Installation and Configuration
4.Program flow
5.Project Questions


INTRODUCTION
					
Our project does the following :-

LucasPyramid: We are required to find the sum of the squares of the k consecutive numbers and try to determine that whether the result that we have just obtained is itself a perfect square i.e. a Square Pyramid.
	
	
REQUIREMENTS
1) .NETCore installed 
2) FSharp
3) Visual Studio	

INSTALLATION AND CONFIGURATION
					
1) After installing .NET core create a new project with the command "dotnet new console -lang "F#" -o project1"
2) Open visual code in the project1 folder by "cd project1" and "code . "
3) Delete .fs file and create a new file with name proj1.fsx
4) Copy all the code from proj1.fsx file in the zip and paste it there
5) Run the command by " dotnet fsi langversion:preview proj1.fsx 1000000 24 "

	
PROGRAM FLOW
    
1) For each actor, the work range is computed by dividing N by the total number of
actors and work range changes according to the value of N. After these
calculations, the main ​ BossActor is recursively spawning the given number of
subordinate actors and is passing each of them the K value and a specific start
range and end range.
2) The ​ ActorCreator class inherits the ​ Actor interface inside which it iteratively
checks for the given work range whether Lucas' Square Pyramid values exist or
not and prints those values on the screen.
3) Once all the subordinate actors have finished their assigned work, and when the
end range of the last actor matches N, the program gets terminated and displays
the CPU time and REAL time.




PROJECT QUESTIONS

Q-1 Size of the work unit that you determined results in best performance for your implementation and an explanation on how you determined it. Size of the work unit refers to the number of sub-problems that a worker gets in a single request from the Supervisor.

Ans: For the given problem, firstly we are taking N and K values from the command line. ​ We are dividing the workload (N) among 30 actors since we observed that it
gave a better performance for our program.

	
Q-2 The result of running your program for

Input: mix run proj1.exs 1000000 4
Output- 
		
For the given N and K values, since there exist no possible Lucas' Square Pyramid values, no output is printed on the screen.

Q-3 The running time for the above as reported by time for the above, i.e run time scala project1.scala 1000000 4 and report the time. The ratio of CPU time to REAL TIME tells you how many cores were effectively used in the computation. If your are close to 1 you have almost no parallelism (points will be subtracted).

We ran our programs on Ubuntu OS with 8 cores and for the above problem the
CPU time is 00:00:24.190 and the REAL time is 00:00:05.134.The ratio of CPU time to
REAL time is 4.71

Q-4	The largest problem you managed to solve.
Ans: The largest problem we managed to solve is N = 100000000 and K = 24 and the ratio of CPU time to REAL time is 3.75