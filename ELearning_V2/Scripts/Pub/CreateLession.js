var CreateLessionApp = angular.module("CreateLessionApp", []);

CreateLessionApp.directive('ngFiles', ['$parse', function ($parse) {
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

CreateLessionApp.controller('CreateLessionController', function ($scope, $sce, $http, $window) {
    $scope.InitUserID = function (UserID) {
        $scope.UserID = UserID;
        LoadTopic();
        EditUrl();
        $scope.UrlRegex = '(?:https?:\/\/)?(?:www\.)?(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))((\w|-){11})?';
    }
    var formdata = new FormData();
    var file;

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
        //var a = angular.element("#Content").val();
        var data = CKEDITOR.instances.Content.getData();
        console.log(data);
        if ($scope.URL != null && $scope.URL != '') {
            var url = $scope.URL.replace("watch?v=", "embed/");
        }
        else {
            var url = '';
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
        var LessionDTO = {
            Name: $scope.Name,
            URL: url,
            Content: CKEDITOR.instances.Content.getData(),
            UserID: $scope.UserID,
            Topics: Topics
        };
        console.log(LessionDTO);

        $http({
            method: 'POST',
            url: '/Lop/CreateLession',
            data: JSON.stringify(LessionDTO)
        }).then(function successCallback(response) {
            if ($('#file').get(0).files.length > 0) {
                ID = response.data;
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
            $window.location.href = '/Lop/LessionDetail/' + response.data;

        });
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
    function EditUrl() {
        document.getElementById('URL').onblur = function () {
            if ($scope.form.input.$valid) {
                var a = document.getElementById('URL').value;
                var b = a.replace("watch?v=", "embed/");
                console.log(b);
                document.getElementById('Video').src = b;
            }
        }
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
});
