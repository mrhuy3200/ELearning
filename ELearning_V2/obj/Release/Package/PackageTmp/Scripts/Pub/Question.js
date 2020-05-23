var CreateQuestionApp = angular.module("CreateQuestionApp", []);

CreateQuestionApp.directive('ngFiles', ['$parse', function ($parse) {
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

CreateQuestionApp.controller('CreateQuestionController', function ($scope, $http, $sce, $window) {
    $scope.Index = -1;
    InitCss();
    LoadTopic();
    var formdata = new FormData();
    var file;
    function InitCss() {
        var x = document.getElementsByClassName("cke_textarea_inline");
        for (var i = 0; i < 2; i++) {
            x[i].style.border = "2px solid";
            x[i].style.position = "relative";
            x[i].style.height = "auto";
            x[i].style.borderRadius = "10px";
            x[i].style.backgroundColor = "snow";
            x[i].style.minHeight = "100px";
            x[i].style.padding = "2px 10px";

        }
        x[2].style.border = "2px solid";
        x[2].style.position = "relative";
        x[2].style.height = "auto";
        x[2].style.borderRadius = "10px";
        x[2].style.backgroundColor = "snow";
        x[2].style.minHeight = "44px";
        x[2].style.padding = "2px 10px";

    }
    $scope.Answers = [];
    $scope.FAnswers = [];
    $scope.AddAnswer = function () {
        if ($scope.Index == -1) {
            var Answer = CKEDITOR.instances.AContent.getData();
            var FAnswer = $sce.trustAsHtml(Answer);
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
            $scope.Answers[$scope.Index] = CKEDITOR.instances.AContent.getData();
            $scope.FAnswers[$scope.Index] = $sce.trustAsHtml($scope.Answers[$scope.Index]);
            $scope.Index = -1;
            CKEDITOR.instances.AContent.setData('');
            setTimeout(function () {
                MathJax.Hub.Queue(["Typeset", MathJax.Hub]);
            }, 200);
        }
    }
    $scope.RemoveAnswer = function (Index) {
        $scope.FAnswers.splice(Index, 1);
        $scope.Answers.splice(Index, 1);
    }
    $scope.EditAnswer = function (Index) {
        CKEDITOR.instances.AContent.setData($scope.Answers[Index]);
        $scope.Index = Index;

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
            var ListAns = [];
            for (var i = 0; i < $scope.Answers.length; i++) {
                var AnswerDTO = {
                    Content: $scope.Answers[i]
                }
                ListAns.push(AnswerDTO);
            }
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

            var QuestionDTO = {
                Content: CKEDITOR.instances.QContent.getData(),
                AnswerID: $("input[name='RightAnswer']:checked").val(),
                Level: $("#Level").val(),
                Solution: CKEDITOR.instances.SContent.getData(),
                Answers: ListAns,
                Topics: Topics
            };
            console.log(QuestionDTO);
            $http({
                method: 'POST',
                url: '/Lop/CreateQuestion',
                data: JSON.stringify(QuestionDTO)
            }).then(function successCallback(response) {
                if (response.data == 1) {
                    var c = confirm("Lưu thành công, tiếp tục tạo câu hỏi", "Thông báo");
                    if (c == false) {
                        $window.location.href = '/Lop/DayThem#NganHangCauHoi';
                    }
                    clear();
                }
                if (response.data == -1) {
                    alert("Lưu thất bại");
                    //$window.location.href = '/Lop/DayThem';
                }
            });
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
    var topicFlag = 0;
    $scope.CreateTopic = function () {
        var TopicDTO = {
            Name: $scope.TopicName
        };
        if ($scope.TopicName != null && $scope.TopicName != "" && topicFlag == 0) {
            $http({
                method: 'POST',
                url: '/Lop/CreateTopic',
                data: JSON.stringify(TopicDTO)
            }).then(function successCallback(response) {
                if (response.data != 0) {
                    TopicDTO["ID"] = response.data;
                    console.log(TopicDTO);
                    $scope.Topics.push(TopicDTO);
                    $scope.TopicName = "";
                }
                else {
                    alert("Fail");
                }
            });
        }

        if ($scope.TopicName != null && $scope.TopicName != "" && topicFlag != 0) {
            EditTopic(topicFlag);
        }
    }
    $scope.SelectTopic = function (Index) {
        document.getElementById("TopicInput").value = $scope.Topics[Index].Name;
        topicFlag = Index;
    }
    function EditTopic(Index) {
        var TopicDTO = $scope.Topics[Index];
        TopicDTO.Name = $scope.TopicName;
        $http({
            method: 'POST',
            url: '/Lop/EditTopic',
            data: JSON.stringify(TopicDTO)
        }).then(function successCallback(response) {
            if (response.data == 1) {
                $scope.Topics[Index].Name = $scope.TopicName;
                $scope.TopicName = "";
            }
            else {
                alert("Fail");
            }
        });

    }
    $scope.DeleteTopic = function (Index) {
        console.log($scope.Topics[Index]);
        $http({
            method: 'POST',
            url: '/Lop/DeleteTopic',
            data: JSON.stringify($scope.Topics[Index])
        }).then(function successCallback(response) {
            if (response.data == 1) {
                $scope.Topics.splice(Index, 1);
            }
            else {
                alert("Fail");
            }
        });

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

