var TestDetailApp = angular.module("TestDetailApp", ['angularUtils.directives.dirPagination']);

TestDetailApp.controller('TestDetailController', function ($scope, $http, $window, $sce) {
    $scope.InitTestID = function (TestID) {
        $scope.QcurrentPage = 1;
        $scope.QpageSize = 5;
        $scope.TestID = TestID;
        LoadTest(TestID);
        LoadQuestion();
        LoadTopic();
        setTimeout(function () {
            console.log("Question" + $scope.Questions.length);
            var temp = $scope.Test.AmountQuestion - $scope.Test.Questions.length;
            console.log("Test" + temp);

            if ($scope.Questions.length < temp) {
                $scope.MaxRandom = $scope.Questions.length;
            }
            else {
                $scope.MaxRandom = temp;
            }
        }, 5000);
    }
    $scope.lstTopic = [];
    function LoadTest(TestID) {
        $http({
            method: 'GET',
            url: '/Lop/GetTest/' + TestID
        }).then(function (r) {
            if (r.data == 0) {
                alert("Không đủ quyền hạn");
            }
            else {
                $scope.Test = r.data;
                for (var i = 0; i < $scope.Test.Questions.length; i++) {
                    $scope.Test.Questions[i]["FContent"] = $sce.trustAsHtml($scope.Test.Questions[i]["Content"]);
                }
                console.log("Test");
                console.log($scope.Test);
            }
        });
    }
    function LoadQuestion() {
        $http({
            method: 'GET',
            url: '/Lop/GetListQuesionByTestID/' + $scope.TestID
        }).then(function (r) {
            if (r.data == null) {
                alert("Không có dữ liệu câu hỏi");
            }
            else {
                $scope.Questions = r.data;
                for (var i = 0; i < $scope.Questions.length; i++) {
                    $scope.Questions[i]["FContent"] = $sce.trustAsHtml($scope.Questions[i]["Content"]);
                    var charcode = 65;
                    for (var j = 0; j < $scope.Questions[i].Answers.length; j++) {
                        if ($scope.Questions[i].AnswerID == $scope.Questions[i].Answers[j].ID) {
                            $scope.Questions[i]["DapAn"] = String.fromCharCode(charcode);
                        }
                        $scope.Questions[i].Answers[j]["FContent"] = $sce.trustAsHtml($scope.Questions[i].Answers[j].Content);
                        $scope.Questions[i].Answers[j]["Char"] = String.fromCharCode(charcode);
                        charcode++;
                    }
                }
                console.log("Questions");
                console.log($scope.Questions);
                setTimeout(function () {
                    MathJax.Hub.Queue(["Typeset", MathJax.Hub]);
                }, 200);
            }
        });
    }
    function LoadTopic() {
        $http({
            method: 'GET',
            url: '/Lop/GetTopic'
        }).then(function (r) {
            if (r.data == null) {
                alert("Không có dữ liệu câu hỏi");
            }
            else {
                $scope.Topics = r.data;
                console.log("Topics");
                console.log($scope.Topics);
            }
        });
    }
    function UpdateQuestionStatus() {
        for (var i = 0; i < $scope.Questions.length; i++) {
            var lstTopicID = $scope.Questions[i].Topics;
            for (var j = 0; j < lstTopicID.length; j++) {
                $scope.Questions[i]["Hide"] = true;
                var res = $scope.lstTopic.indexOf(lstTopicID[j].ID)
                if (res != -1) {
                    $scope.Questions[i]["Hide"] = false;
                    break;
                }
            }
        }
    }
    function FindQuestion(QuestionID) {
        for (var i = 0; i < $scope.Questions.length; i++) {
            if ($scope.Questions[i].ID == QuestionID) {
                return i;
            }
        }
    }
    function FindTestQuestion(QuestionID) {
        for (var i = 0; i < $scope.Test.Questions.length; i++) {
            if ($scope.Test.Questions[i].ID == QuestionID) {
                return i;
            }
        }
    }
    $scope.CheckTopic = function (Index) {
        var TopicID = parseInt($("#check" + Index).val());
        console.log($scope.lstTopic);
        if ($scope.lstTopic.indexOf(TopicID) == -1) {
            $scope.lstTopic.push(TopicID);
            UpdateQuestionStatus();
        }
        else {
            $scope.lstTopic.splice($scope.lstTopic.indexOf(TopicID), 1);
            UpdateQuestionStatus();
        }
        console.log($scope.lstTopic);
    }
    $scope.AddQuestionToTest = function (QuestionID) {
        if ($scope.Test.Questions.length < $scope.Test.AmountQuestion) {
            var TestQuestionDTO = {
                TestID: $scope.TestID,
                QuestionID: QuestionID
            };
            $http({
                method: 'POST',
                url: '/Lop/AddQuestionToTest',
                data: JSON.stringify(TestQuestionDTO)
            }).then(function (r) {
                if (r.data == 1) {
                    $scope.Test.Questions.push($scope.Questions[FindQuestion(QuestionID)]);
                    $scope.Questions.splice(FindQuestion(QuestionID), 1)
                    //$scope.Questions[FindQuestion(QuestionID)]["Hide"] = true;
                }
                if (r.data == 0) {
                    alert("Không đủ quyền hạn");
                }
                if (r.data == -2) {
                    alert("Xảy ra lỗi");
                }
                if (r.data == -1) {
                    alert("Câu hỏi đã được thêm trước đó");
                }
            });
        }
    }
    $scope.RemoveQuestionFromTest = function (QuestionID) {
        var TestQuestionDTO = {
            TestID: $scope.TestID,
            QuestionID: QuestionID
        };
        $http({
            method: 'POST',
            url: '/Lop/RemoveQuestionFromTest',
            data: JSON.stringify(TestQuestionDTO)
        }).then(function (r) {
            if (r.data == 1) {
                $scope.Questions.push($scope.Test.Questions[FindTestQuestion(QuestionID)]);
                $scope.Test.Questions.splice(FindTestQuestion(QuestionID), 1);
            }
            if (r.data == 0) {
                alert("Không đủ quyền hạn");
            }
            if (r.data == -2) {
                alert("Xảy ra lỗi");
            }
            if (r.data == -1) {
                alert("Câu hỏi chưa dược thêm trước đó");
            }
        });
    }
    $scope.RandomTestQuestion = function () {
        var CauHoiDeThi = {
            ChuongID: $scope.TopicRandom,
            DoKho: $scope.LevelRandom,
            SoCauHoi: $scope.AmountRandom,
            DeThiID: $scope.TestID
        }
        $http({
            method: 'POST',
            url: '/Lop/RandomTestQuestion',
            data: JSON.stringify(CauHoiDeThi)
        }).then(function (r) {
            if (r.data != null) {
                console.log(r.data);
                for (var i = 0; i < r.data.length; i++) {
                    r.data[i]["FContent"] = $sce.trustAsHtml(r.data[i]["Content"]);
                    $scope.Test.Questions.push(r.data[i]);
                    $scope.Questions.splice(FindQuestion(r.data[i].ID), 1)
                    setTimeout(function () {
                        MathJax.Hub.Queue(["Typeset", MathJax.Hub]);
                    }, 200);
                }
                $scope.TopicRandom = '';
                $scope.LevelRandom = '';
                $scope.AmountRandom = '';
            }
            if (r.data == 0) {
                alert("Không đủ quyền hạn");
            }
            if (r.data == -1) {
                alert("Không đủ câu hỏi");
            }
        });
    }


});
