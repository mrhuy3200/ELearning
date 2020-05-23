var DoTestApp = angular.module("DoTestApp", []);

DoTestApp.controller('DoTestController', function ($scope, $sce, $http, $window, DoTestService) {
    $scope.Init = function (CourseID, UserID, TestID) {
        console.log("UserID: " + UserID);
        $scope.CourseID = CourseID;
        $scope.UserID = UserID;
        $scope.TestID = TestID;
        $scope.Start = 0;
        LoadTest(LoadTestResult);
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
        $("#Timer").css("display", "none");
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
            var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((distance % (1000 * 60)) / 1000);

            // Output the result in an element with id="demo"
            document.getElementById("Timer").innerHTML = hours + "h "
                + minutes + "m " + seconds + "s ";

            // If the count down is over, write some text 
            if (distance < 0) {
                clearInterval(x);
                document.getElementById("Timer").innerHTML = "EXPIRED";
                alert("Time Out");
            }
        }, 1000);
    }
    function LoadTestResult() {
        var TestResultDTO = {
            UserID: $scope.UserID,
            TestID: $scope.TestID,
            CourseID: $scope.CourseID
        }
        DoTestService.LoadTestResult(TestResultDTO).then(function (res) {
            if (res.data != -1) {
                $scope.Test["Result"] = res.data;
                $scope.TestDone = true;
                console.log($scope.Test);
                setTimeout(function () {
                    MathJax.Hub.Queue(["Typeset", MathJax.Hub]);
                }, 200);
                setTimeout(function () {
                    for (var i = 0; i < $scope.Test.Questions.length; i++) {
                        for (var j = 0; j < $scope.Test.Questions[i].Answers.length; j++) {
                            var id = "#Answer" + i + j;
                            if ($scope.Test.Questions[i].Answers[j].ID == $scope.Test.Questions[i].AnswerID) {
                                $(id).css('background-color', 'lightgreen');
                            }
                            if ($scope.Test.Questions[i].Answers[j].ID == $scope.Test.Result.TestDetails[i].Answer && $scope.Test.Result.TestDetails[i].Answer != $scope.Test.Questions[i].AnswerID) {
                                $(id).css('background-color', 'antiquewhite');
                            }
                        }
                        if ($scope.Test.Result.TestDetails[i].Answer == 0) {
                            //Người dùng k chọn đáp án
                        }
                    }
                }, 200);

            }
            else {
                $scope.Test["Result"] = '';
                $scope.TestDone = false;
                console.log(document.getElementById("Timer"));

                document.getElementById("Timer").innerHTML = $scope.Test.Time + ":00"

            }
            console.log($scope.TestDone);
        })
    }
    function LoadTest(callback) {
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
                callback();
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
                alert("Lưu bài làm thành công");
                $window.location.href = "/Lop/ViewCourse/" + $scope.CourseID;

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
    fac.LoadTestResult = function (TestResultDTO) {
        return $http({
            method: 'POST',
            url: '/Course/GetTestResult',
            data: JSON.stringify(TestResultDTO)
        });
    };
    fac.SubmitTest = function (TestDetails) {
        return $http({
            method: 'POST',
            url: '/Course/SubmitTest',
            data: JSON.stringify(TestDetails)
        });
    }
    return fac;
});

