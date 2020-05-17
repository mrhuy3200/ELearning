var EditLessionApp = angular.module("EditLessionApp", []);

EditLessionApp.directive('ngFiles', ['$parse', function ($parse) {
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

EditLessionApp.controller('EditLessionController', function ($scope, $http, $window, $sce) {
    var formdata = new FormData();
    var file;
    $scope.InitLessionID = function (id) {
        $scope.LessionID = id;
        InitCss();
        LoadTopic();
        setTimeout(function () {
            LoadLession();
        }, 1000);

    }
    function InitCss() {
        console.log("OK");
        var x = document.getElementsByClassName("cke_textarea_inline");
        x[0].style.display = "none";
        x[1].style.border = "2px solid";
        x[1].style.position = "relative";
        x[1].style.height = "auto";
        x[1].style.borderRadius = "10px";
        x[1].style.backgroundColor = "snow";
        x[1].style.minHeight = "100px";
        x[1].style.padding = "2px 10px";
    }
    function LoadLession() {
        $http({
            method: 'GET',
            url: '/Lop/GetLessionByID/' + $scope.LessionID
        }).then(function successCallback(response) {
            console.log(response.data);
            $scope.Lession = response.data;
            CKEDITOR.instances.Content.setData(response.data.Content);
            CKEDITOR.instances.Test.setData(response.data.Content);

            $scope.URL = $sce.trustAsResourceUrl($scope.Lession.URL);
            SetUpTopic($scope.Lession.Topics);
            console.log("Set Content" + response.data.Content);


        });
    }

    $scope.getTheFiles = function ($files) {
        console.log($files)
        file = $files[0];
        console.log(formdata);
    };
    $scope.save = function () {
        //var a = angular.element("#Content").val();
        var url = $scope.Lession.URL;
        $scope.Lession.URL = url.replace("watch?v=", "embed/");
        $scope.Lession.Content = CKEDITOR.instances.Content.getData();
        var Topics = [];
        var lstTopic = $("input[name='CheckTopic']:checked");
        console.log(lstTopic)
        for (var i = 0; i < lstTopic.length; i++) {
            var Topic = {
                ID: lstTopic[i].value
            };
            Topics.push(Topic);
        }
        $scope.Lession.Topics = Topics;
        console.log($scope.Lession);

        $http({
            method: 'POST',
            url: '/Lop/EditLession',
            data: JSON.stringify($scope.Lession)
        }).then(function successCallback(response) {
            if (file != null) {
                ID = $scope.Lession.ID;
                console.log(ID);
                console.log('filename ' + file.name);
                var blob = file.slice(0, file.size, 'image/jpg');
                newFile = new File([blob], ID + '.jpg', { type: 'image/jpg' });
                console.log('filename ' + newFile);
                formdata.set(0, newFile);
                console.log(formdata.get(0));
                $http({
                    method: 'POST',
                    url: '/api/API/UploadLessionImage',
                    data: formdata,
                    headers: {
                        'Content-Type': undefined
                    }
                }).then(function successCallback(response) {
                    alert(response.data);
                    if (response.data == "OK") {
                        $window.location.href = '/Lop/LessionDetail/' + ID;
                    }
                });

            }
            else {
                alert(response.data)
                if (response.data == "OK") {
                    $window.location.href = '/Lop/LessionDetail/' + $scope.Lession.ID;
                }
            }
        });
    }
    //TopicManage////////////////////////////////////
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
    function FindTopic(TopicID) {
        for (var i = 0; i < $scope.Topics.length; i++) {
            if ($scope.Topics[i].ID == TopicID) {
                return i;
            }
        }
    }
    function SetUpTopic(ListTopic) {
        for (var i = 0; i < ListTopic.length; i++) {

            var topicID = "check" + FindTopic(ListTopic[i].ID);
            document.getElementById(topicID).checked = true;
        }
    }
    //TopicManage////////////////////////////////////

});
