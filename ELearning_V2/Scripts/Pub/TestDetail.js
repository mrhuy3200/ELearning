var TestDetailApp = angular.module("TestDetailApp", ['angularUtils.directives.dirPagination']);

TestDetailApp.controller('TestDetailController', function ($scope, $http, $window, $sce) {
    $scope.InitTestID = function (TestID) {
        $scope.QcurrentPage = 1;
        $scope.QpageSize = 5;
        $scope.TestID = TestID;
        LoadTest(TestID);
        LoadQuestion();
        LoadTopic();
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
            url: '/Lop/GetListQuesionByUserID'
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
    $scope.CheckTopic = function (Index) {
        var TopicID = $("#check" + Index).val();
        console.log($scope.lstTopic)
        if ($scope.lstTopic.indexOf(TopicID) == -1) {
            $scope.lstTopic.push(TopicID);
        }
        else {
            $scope.lstTopic.splice($scope.lstTopic.indexOf(TopicID),1)
        }
        console.log($scope.lstTopic)

    }

});
