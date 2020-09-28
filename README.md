Q-1 Size of the work unit that you determined results in best performance for your
implementation and an explanation on how you determined it. Size of the work unit
refers to the number of subproblems that a worker gets in a single request from the
boss.
Ans:
● For the given problem, firstly we are taking N and K values from the command
line. ​ We are dividing the workload (N) among 30 actors since we observed that it
gave a better performance for our program.
● For each actor, the work range is computed by dividing N by the total number of
actors and work range changes according to the value of N. After these
calculations, the main ​ BossActor is recursively spawning the given number of
subordinate actors and is passing each of them the K value and a specific start
range and end range.
● The ​ ActorCreator class inherits the ​ Actor interface inside which it iteratively
checks for the given work range whether Lucas' Square Pyramid values exist or
not and prints those values on the screen.
● Once all the subordinate actors have finished their assigned work, and when the
end range of the last actor matches N, the program gets terminated and displays
the CPU time and REAL time.
Q-2 The result of running your program for input: ​ dotnet fsi proj1.fsx 1000000 4
Ans: For the given N and K values, since there exist no possible Lucas' Square
Pyramid values, no output is printed on the screen.
Q-3 The running time for the above as reported by time for the above, i.e run ​ time
scala project1.scala 1000000 4 and report the time. The ratio of CPU time to
REAL TIME tells you how many cores were effectively used in the computation. If your
are close to 1 you have almost no parallelism (points will be subtracted).
Ans: We ran our programs on Ubuntu OS with 8 cores and for the above problem the
CPU time is 00:00:24.190 and the REAL time is 00:00:05.134.The ratio of CPU time to
REAL time is 4.71Q-4 The largest problem you managed to solve.
Ans: The largest problem we managed to solve is N = 100000000 and K = 24 and the
ratio of CPU time to REAL time is 3.75