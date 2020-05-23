var DoTestApp = angular.module("DoTestApp", []);

DoTestApp.controller('DoTestController', function ($scope, $sce, $http, $window, DoTestService) {
    $scope.Init = function (CourseID, UserID, TestID) {
        console.log("UserID: " + UserID);
        $scope.CourseID = CourseID;
        $scope.UserID = UserID;
        $scope.TestID = TestID;
        $scope.Start = 0;
        LoadTest();
        //downWaiting();
    };
    $scope.StartTest = function () {
        $scope.Questions = $scope.Test.Questions;
        $scope.Start = 1;
        SetTime($scope.Test.Time);
        console.log($scope.Questions);
    }
    $scope.EndTest = function () {
        EndTest();
        $scope.Start = 2;
    }
    $scope.ViewTest = function () {
        $window.location.href = "/Course/ViewTestResult/" + $scope.TestID;
    }
    function SetTime(Time) {
        var countDownDate = new Date().getTime() + Time * 60000;

        // Update the count down every 1 second
        var x = setInterval(function () {

            // Get today's date and time
            var now = new Date().getTime();

            // Find the distance between now and the count down date
            var distance = countDownDate - now;

            // Time calculations for days, hours, minutes and seconds

            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((distance % (1000 * 60)) / 1000);

            // Output the result in an element with id="demo"
            document.getElementById("Timer").innerHTML = minutes + "m " + seconds + "s ";

            // If the count down is over, write some text 
            if (distance < 0) {
                clearInterval(x);
                document.getElementById("Timer").innerHTML = "EXPIRED";
                alert("Time Out");
            }
        }, 1000);
    }
    function LoadTest() {
        var TestResultDTO = {
            UserID: $scope.UserID,
            TestID: $scope.TestID,
            CourseID: $scope.CourseID
        }
        DoTestService.LoadTest(TestResultDTO).then(function (r) {
            if (r.data != '') {
                for (var i = 0; i < r.data.Questions.length; i++) {
                    r.data.Questions[i].Content = $sce.trustAsHtml(r.data.Questions[i].Content);
                    var charcode = 65;
                    for (var j = 0; j < r.data.Questions[i].Answers.length; j++) {
                        r.data.Questions[i].Answers[j].Content = $sce.trustAsHtml(r.data.Questions[i].Answers[j].Content);
                        r.data.Questions[i].Answers[j]["Char"] = String.fromCharCode(charcode);
                        charcode++;
                    }
                }
                $scope.Test = r.data;
                $scope.Start = false;
                document.getElementById("Timer").innerHTML = r.data.Time + ":00"
                console.log(r.data);
            }
        })
    }
    function EndTest() {
        var TestDetails = [];

        for (var i = 0; i < $scope.Questions.length; i++) {
            var TestDetail = {
                QuestionID: $scope.Questions[i].ID,
                Answer: $('input[name =' + 'Question' + i + ']:checked').val(),
                TestID: $scope.Test.ID,
                UserID: $scope.UserID
            }
            TestDetails.push(TestDetail);
        }
        console.log("Res");
        console.log(TestDetails);
        var TestResultDTO = {
            UserID: $scope.UserID,
            TestID: $scope.TestID,
            CourseID: $scope.CourseID,
            TestDetails: TestDetails
        }
        DoTestService.SubmitTest(TestResultDTO).then(function (r) {
            if (r.data != '') {
                $scope.Questions = '';

            }
        })
    }
});




DoTestApp.factory('DoTestService', function ($http) {
    var fac = {};
    fac.LoadTest = function (TestResultDTO) {
        return $http({
            method: 'POST',
            url: '/Course/GetTestByID',
            data: JSON.stringify(TestResultDTO)
        });
    };
    fac.LoadTestResult = function (TestDetails) {
        return $http({
            method: 'POST',
            url: '/Course/SubmitTest',
            data: JSON.stringify(TestDetails)
        });
    }
    return fac;
});

