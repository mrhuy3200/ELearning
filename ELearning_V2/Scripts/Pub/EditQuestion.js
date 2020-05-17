var EditQuestionApp = angular.module("EditQuestionApp", []);

EditQuestionApp.directive('ngFiles', ['$parse', function ($parse) {
    function fn_link(scope, element, attrs) {
        var onChange = $parse(attrs.ngFiles);
        element.on('change', function (event) {
            onChange(scope, { $files: event.target.files });
        });
    };

    return {
        link: fn_link
    }
}])

EditQuestionApp.controller('EditQuestionController', function ($scope, $http, $sce, $window) {
    $scope.QuestionID = function (QuestionID) {
        $scope.QuestionID = QuestionID;
        $scope.Index = -1;
        InitCss();
        LoadTopic();
        setTimeout(function () {
            GetQuestion();
        }, 1000);
    }
    $(document).ready(function () {
        console.log("ready!");
    });
    $scope.FAnswers = [];
    $scope.Answers = [];
    $scope.GetQuestion = function () {
        GetQuestion();
    }
    function GetQuestion() {
        console.log("GetQuestion!");

        $http({
            method: 'GET',
            url: '/Lop/GetQuestion/' + $scope.QuestionID
        }).then(function successCallback(response) {
            if (response.data != -1) {
                $scope.Question = response.data;
                console.log($scope.Question);
                CKEDITOR.instances.QContent.setData(response.data.Content);
                CKEDITOR.instances.SContent.setData(response.data.Solution);
                $scope.Answers = response.data.Answers;
                console.log("Answers: " + $scope.Answers);
                for (var i = 0; i < $scope.Question.Answers.length; i++) {
                    var FAnswer = $sce.trustAsHtml($scope.Question.Answers[i].Content);
                    $scope.FAnswers.push(FAnswer);
                    if (response.data.AnswerID == $scope.Question.Answers[i].ID) {
                        var id = "radio" + i
                    }
                }
                for (var i = 0; i < $scope.Question.Topics; i++) {

                }
                $('#Level').val(response.data.Level).niceSelect('update');

                setTimeout(function () {
                    MathJax.Hub.Queue(["Typeset", MathJax.Hub]);
                    document.getElementById(id).checked = true;
                }, 200);
            }
            else {
                alert("Không tìm thấy dữ liệu");
            }
        });
    }
    function FindTopic(TopicID) {
        for (var i = 0; i < $scope.Topics.length; i++) {
            if ($scope.Topics[i].ID == TopicID) {
                return i;
            }
        }
    }
    function InitCss() {
        console.log("OK");
        var x = document.getElementsByClassName("cke_textarea_inline");
        x[0].style.display = "none";
        for (var i = 0; i < 2; i++) {
            x[i+1].style.border = "2px solid";
            x[i+1].style.position = "relative";
            x[i+1].style.height = "auto";
            x[i+1].style.borderRadius = "10px";
            x[i+1].style.backgroundColor = "snow";
            x[i+1].style.minHeight = "100px";
            x[i+1].style.padding = "2px 10px";

        }
        x[3].style.border = "2px solid";
        x[3].style.position = "relative";
        x[3].style.height = "auto";
        x[3].style.borderRadius = "10px";
        x[3].style.backgroundColor = "snow";
        x[3].style.minHeight = "44px";
        x[3].style.padding = "2px 10px";


    }
    $scope.Answers = [];
    $scope.FAnswers = [];
    $scope.AddAnswer = function () {
        console.log($scope.Index);
        if ($scope.Index == -1) {
            var Answer = {
                ID: 0,
                Content: CKEDITOR.instances.AContent.getData()
            }
            var FAnswer = $sce.trustAsHtml(Answer.Content);

            console.log(FAnswer);
            $scope.Answers.push(Answer);
            $scope.FAnswers.push(FAnswer);
            if ($scope.Answers.length >= 2) {
                $("#AContentVal").text("");
            }
            CKEDITOR.instances.AContent.setData('');
            setTimeout(function () {
                MathJax.Hub.Queue(["Typeset", MathJax.Hub]);
            }, 200);
        }
        else {
            $scope.Answers[$scope.Index].Content = CKEDITOR.instances.AContent.getData();
            $scope.FAnswers[$scope.Index] = $sce.trustAsHtml($scope.Answers[$scope.Index].Content);
            $scope.Index = -1;
            CKEDITOR.instances.AContent.setData('');
            setTimeout(function () {
                MathJax.Hub.Queue(["Typeset", MathJax.Hub]);
            }, 200);
        }
    }

    //$scope.InitCourseID = function (id) {
    //    $scope.CourseID = id;
    //}
    $scope.EditAnswer = function (Index) {
        CKEDITOR.instances.AContent.setData($scope.Answers[Index].Content);
        $scope.Index = Index;
        //CKEDITOR.instances.QContent.setData($scope.Question.Content);
        //CKEDITOR.instances.SContent.setData($scope.Question.Solution);

    }

    $scope.RemoveAnswer = function (Index) {
        $scope.FAnswers.splice(Index, 1);
        $scope.Answers.splice(Index, 1);
    }

    $scope.getTheFiles = function ($files) {
        console.log($files);
        angular.forEach($files, function (value, key) {
            console.log(key + ' ' + value.name);
            formdata.set(key, value);
        });
        file = $files[0];
        console.log(formdata);
    };
    $scope.save = function () {
        if (Validate()) {
            var ListAns = $scope.Answers;
            //for (var i = 0; i < $scope.Answers.length; i++) {
            //    var AnswerDTO = {
            //        Content: $scope.Answers[i]
            //    }
            //    ListAns.push(AnswerDTO);
            //}
            $scope.Question.Content = CKEDITOR.instances.QContent.getData();
            $scope.Question.AnswerID = $("input[name='RightAnswer']:checked").val();
            $scope.Question.Level = $("#Level").val();
            $scope.Question.Solution = CKEDITOR.instances.SContent.getData();
            $scope.Question.Answers = $scope.Answers;
            var Topics = [];
            var lstTopic = $("input[name='CheckTopic']:checked");
            console.log(lstTopic)
            for (var i = 0; i < lstTopic.length; i++) {
                var Topic = {
                    ID: lstTopic[i].value
                };
                Topics.push(Topic);
            }
            console.log(Topics);
            $scope.Question.Topics = Topics;
            //var QuestionDTO = {
            //    Content: CKEDITOR.instances.QContent.getData(),
            //    AnswerID: $("input[name='RightAnswer']:checked").val(),
            //    Level: $("#Level").val(),
            //    Solution: CKEDITOR.instances.SContent.getData(),
            //    Answers: ListAns
            //};
            console.log($scope.Question);
            //$http({
            //    method: 'POST',
            //    url: '/Lop/EditQuestion',
            //    data: JSON.stringify($scope.Question)
            //}).then(function successCallback(response) {
            //    if (response.data == 1) {
            //        alert("Cập nhật thành công")
            //        $window.location.href = '/Lop/DayThem#NganHangCauHoi';
            //    }
            //    if (response.data == -1) {
            //        alert("Lưu thất bại");
            //        //$window.location.href = '/Lop/DayThem';
            //    }
            //    if (response.data == 0) {
            //        alert("Không đủ quyền hạn")
            //        $window.location.href = '/Lop/DayThem#NganHangCauHoi';
            //    }
            //});
        }
    }
    $scope.clear = function () {
        clear();
    }
    function clear() {
        CKEDITOR.instances.QContent.setData("");
        CKEDITOR.instances.SContent.setData("");
        CKEDITOR.instances.AContent.setData("");
        $scope.Answers = [];
        $scope.FAnswers = [];
        $('#Level').val(0).niceSelect('update');
    }
    function FindAnswer(AnswerID) {
        for (var i = 0; i < $scope.Question.Answers.length; i++) {
            if ($scope.Question.Answers[i].ID == AnswerID) {
                return $scope.Question.Answers[i];
            }
        }
    }
    function LoadTopic() {
        $http({
            method: 'GET',
            url: '/Lop/GetTopic'
        }).then(function successCallback(response) {
            $scope.Topics = response.data;
            console.log("Topics: " + $scope.Topics)
        });
    }
    function Validate() {
        if (CKEDITOR.instances.QContent.getData() == '') {
            console.log(CKEDITOR.instances.QContent.getData());
            $("#QContentVal").css('display', 'block');
            return false;
        }
        if ($scope.Answers.length < 2) {
            console.log($scope.Answers);
            $("#AContentVal").text("Vui lòng tạo ít nhất 2 đáp án");
            return false;
        }
        if ($("input[name='RightAnswer']:checked").val() == null) {
            console.log($("input[name='RightAnswer']:checked").val());
            $("#AContentVal").text("Vui lòng chọn đáp án đúng");
            return false;
        }
        if ($("#Level").val() == 0) {
            console.log($("#Level").val());
            $("#LevelVal").css('display', 'block');
            return false;
        }
        return true;
    }
});

