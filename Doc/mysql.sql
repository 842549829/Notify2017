/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50162
Source Host           : localhost:3306
Source Database       : db

Target Server Type    : MYSQL
Target Server Version : 50162
File Encoding         : 65001

Date: 2015-11-25 17:15:46
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for account
-- ----------------------------
DROP TABLE IF EXISTS `account`;
CREATE TABLE `account` (
  `Id` varchar(36) NOT NULL,
  `AccountNo` varchar(100) NOT NULL,
  `AccountName` varchar(100) NOT NULL,
  `Mail` varchar(100) NOT NULL,
  `Mobile` varchar(11) NOT NULL,
  `Password` varchar(100) NOT NULL,
  `PayPassword` varchar(100) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `IsAdmin` bit(1) NOT NULL,
  `Status` int(11) NOT NULL COMMENT '0:禁用\r\n            1:启用',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of account
-- ----------------------------
INSERT INTO `account` VALUES ('15b8b9d3-a6a1-4d74-929c-7c46a7e7458f', 'sd', 'ss', 's', 'ss', 'E10ADC3949BA59ABBE56E057F20F883E', 'E10ADC3949BA59ABBE56E057F20F883E', '2015-11-25 16:30:57', '\0', '1');
INSERT INTO `account` VALUES ('3823df7a-7aec-499e-8167-97982ed51f73', 'djfkhja', '5555', '5555', '5555', 'E10ADC3949BA59ABBE56E057F20F883E', 'E10ADC3949BA59ABBE56E057F20F883E', '2015-11-25 16:31:07', '\0', '1');
INSERT INTO `account` VALUES ('717d2f69-ae00-415a-a810-636ae9bb5425', 'x', 'c', 'x', 'x', 'E10ADC3949BA59ABBE56E057F20F883E', 'E10ADC3949BA59ABBE56E057F20F883E', '2015-11-25 16:25:36', '\0', '1');
INSERT INTO `account` VALUES ('da8adcf2-1c5b-4118-9d62-cd69d31602bd', 'admin', '管理员Notify', '1102253039@qq.com', '13551192896', 'E10ADC3949BA59ABBE56E057F20F883E', 'E10ADC3949BA59ABBE56E057F20F883E', '2015-11-23 14:47:57', '', '1');
INSERT INTO `account` VALUES ('e11f525f-e8ce-4e92-99fe-820249d5ea8e', 'a', 'a', 's', 'a', 'E10ADC3949BA59ABBE56E057F20F883E', 'E10ADC3949BA59ABBE56E057F20F883E', '2015-11-25 16:26:09', '\0', '1');

-- ----------------------------
-- Table structure for menu
-- ----------------------------
DROP TABLE IF EXISTS `menu`;
CREATE TABLE `menu` (
  `Id` varchar(36) NOT NULL,
  `Title` varchar(200) NOT NULL,
  `Description` varchar(100) NOT NULL,
  `ParentId` varchar(36) NOT NULL,
  `Url` varchar(200) NOT NULL,
  `Sort` int(11) NOT NULL,
  `Icon` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of menu
-- ----------------------------
INSERT INTO `menu` VALUES ('2b4798cb-9a0d-422f-93c7-882b79c16e89', '城市管理', '11', '3ef60878-ac8b-44d1-945d-45e4f59598eb', '11', '11', 'icon-nav');
INSERT INTO `menu` VALUES ('3891003f-5f74-4b2a-bbf2-2c2854aa702b', '用户列表', '6', '8dc7bb8d-126c-4467-be37-72ca71340b6d', '/User/UserList', '6', 'icon-nav');
INSERT INTO `menu` VALUES ('3d7f82bd-b65c-4db8-9221-cc507c05f4c5', '区域管理', '9', '3ef60878-ac8b-44d1-945d-45e4f59598eb', '9', '9', 'icon-nav');
INSERT INTO `menu` VALUES ('3ef60878-ac8b-44d1-945d-45e4f59598eb', '基础数据管理', '7', '46c7ac23-6c73-49d5-b53d-1b773da5a762', '7', '7', 'icon-sys');
INSERT INTO `menu` VALUES ('46C7AC23-6C73-49D5-B53D-1B773DA5A762', '菜单管理', '菜单管理 ', '00000000-0000-0000-0000-000000000000', '', '1', 'icon-sys');
INSERT INTO `menu` VALUES ('6ff9ecc6-fdb1-48bf-b31f-e7b0701a90eb', '角色管理', '4', '756afd03-4ef3-4fd2-8674-7c73b0f04c8a', '/Role/RoleList', '4', 'icon-nav');
INSERT INTO `menu` VALUES ('756afd03-4ef3-4fd2-8674-7c73b0f04c8a', '权限管理', '权限管理', '46c7ac23-6c73-49d5-b53d-1b773da5a762', 'url', '2', 'icon-sys');
INSERT INTO `menu` VALUES ('87cef101-5189-47f5-adf7-bebf464aa1ed', '县城管理', '12', '3ef60878-ac8b-44d1-945d-45e4f59598eb', '12', '12', 'icon-nav');
INSERT INTO `menu` VALUES ('8dc7bb8d-126c-4467-be37-72ca71340b6d', '用户管理', '用户管理', '46c7ac23-6c73-49d5-b53d-1b773da5a762', '5', '5', 'icon-sys');
INSERT INTO `menu` VALUES ('bae71b5f-66a5-452a-9938-63fdd7140bc4', '菜单管理', '菜单管理', '756afd03-4ef3-4fd2-8674-7c73b0f04c8a', '/Menu/MenuList', '3', 'icon-nav');
INSERT INTO `menu` VALUES ('c175709e-64e5-4c14-ace5-a41891f0de1d', '国家管理', '8', '3ef60878-ac8b-44d1-945d-45e4f59598eb', '8', '8', 'icon-nav');
INSERT INTO `menu` VALUES ('dcad96df-0d63-4968-a310-c16bb2462cf2', '省份管理', '10', '3ef60878-ac8b-44d1-945d-45e4f59598eb', '10', '10', 'icon-nav');

-- ----------------------------
-- Table structure for role
-- ----------------------------
DROP TABLE IF EXISTS `role`;
CREATE TABLE `role` (
  `Id` varchar(36) NOT NULL,
  `RoleName` varchar(100) NOT NULL,
  `RoleDescription` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of role
-- ----------------------------
INSERT INTO `role` VALUES ('0C777E0A-3D1D-429B-82D0-CC5F2F7564C5', 'eee4', 'eee');
INSERT INTO `role` VALUES ('1074A45D-5556-4F88-AB7F-0261B25DA7C4', '权限管理', '权限系统');
INSERT INTO `role` VALUES ('19d4f1d4-2877-4ce5-af24-b8ae2bce5015', 'f', 'da');
INSERT INTO `role` VALUES ('1CB37B77-33EC-4466-A427-B27551D03521', '55', '55');
INSERT INTO `role` VALUES ('1e4a5f96-d4a7-466b-9b8f-2256e6d68993', 'dd', 'f');
INSERT INTO `role` VALUES ('218C1025-38C0-43CB-A682-9540D84DF9E9', 'd4f4p', 'ff');
INSERT INTO `role` VALUES ('31e66e5f-7ed3-4903-b0a6-d42a0cf6f745', 's', 'dds');
INSERT INTO `role` VALUES ('53b902ad-fe5d-4776-8ca3-50761883c290', '14', '134');
INSERT INTO `role` VALUES ('78c9c382-8895-431f-a379-ca7cbc3fc36d', '13', '13');
INSERT INTO `role` VALUES ('81b6a293-9b2c-47e5-ac0e-7a8a6d7bd870', '17', '17');
INSERT INTO `role` VALUES ('8d40f7b4-6e33-4206-a334-45bd98213d68', 'er', 'rr');
INSERT INTO `role` VALUES ('9DE3FC26-F8F9-4DBB-94DF-3B1042FC3536', '测试选中', '5d');
INSERT INTO `role` VALUES ('a8ec06f2-0fd7-476a-847e-a360df1feeca', '默认角色', '默认角色');

-- ----------------------------
-- Table structure for rolepermissions
-- ----------------------------
DROP TABLE IF EXISTS `rolepermissions`;
CREATE TABLE `rolepermissions` (
  `Id` varchar(36) NOT NULL,
  `RoleId` varchar(36) NOT NULL,
  `MenuId` varchar(36) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of rolepermissions
-- ----------------------------
INSERT INTO `rolepermissions` VALUES ('067bcfc7-6f14-44ee-bc02-fabb089b9cd9', '9de3fc26-f8f9-4dbb-94df-3b1042fc3536', 'c175709e-64e5-4c14-ace5-a41891f0de1d');
INSERT INTO `rolepermissions` VALUES ('1334e3ec-e446-4541-a2fe-fe159c0c3649', '9de3fc26-f8f9-4dbb-94df-3b1042fc3536', '3ef60878-ac8b-44d1-945d-45e4f59598eb');
INSERT INTO `rolepermissions` VALUES ('3be5d670-ec85-49a1-8c78-1f1dea7fc621', 'a8ec06f2-0fd7-476a-847e-a360df1feeca', '756afd03-4ef3-4fd2-8674-7c73b0f04c8a');
INSERT INTO `rolepermissions` VALUES ('5746b22e-0f5e-4f9b-8aef-e0d57e5dee67', '9de3fc26-f8f9-4dbb-94df-3b1042fc3536', '87cef101-5189-47f5-adf7-bebf464aa1ed');
INSERT INTO `rolepermissions` VALUES ('5d733d54-fcd2-44ef-aedf-4e8e09521522', 'a8ec06f2-0fd7-476a-847e-a360df1feeca', '46c7ac23-6c73-49d5-b53d-1b773da5a762');
INSERT INTO `rolepermissions` VALUES ('75eef946-6bf8-4612-b0ab-13152c0a6d4c', 'a8ec06f2-0fd7-476a-847e-a360df1feeca', 'bae71b5f-66a5-452a-9938-63fdd7140bc4');
INSERT INTO `rolepermissions` VALUES ('81fca8ee-4cfb-4628-8a90-886fd1969296', '9de3fc26-f8f9-4dbb-94df-3b1042fc3536', '46c7ac23-6c73-49d5-b53d-1b773da5a762');
INSERT INTO `rolepermissions` VALUES ('ffe9aec4-b597-4275-a6d5-96ce892cf680', 'a8ec06f2-0fd7-476a-847e-a360df1feeca', '6ff9ecc6-fdb1-48bf-b31f-e7b0701a90eb');

-- ----------------------------
-- Table structure for roleuserrelationship
-- ----------------------------
DROP TABLE IF EXISTS `roleuserrelationship`;
CREATE TABLE `roleuserrelationship` (
  `Id` varchar(36) NOT NULL,
  `AccountId` varchar(36) NOT NULL,
  `RoleId` varchar(36) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of roleuserrelationship
-- ----------------------------

-- ----------------------------
-- Table structure for verificationcode
-- ----------------------------
DROP TABLE IF EXISTS `verificationcode`;
CREATE TABLE `verificationcode` (
  `Id` varchar(36) NOT NULL,
  `Type` tinyint(4) NOT NULL COMMENT '0:短信\r\n            1:邮箱',
  `Code` varchar(100) NOT NULL,
  `AccountId` varchar(36) NOT NULL,
  `Contact` varchar(100) NOT NULL,
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of verificationcode
-- ----------------------------
INSERT INTO `verificationcode` VALUES ('7e234e16-5cde-417a-b589-f3361a9d5f1c', '1', '51980', '14e0a3d5-7c3c-464c-be62-14ea92d00081', '51980', '2015-11-12 11:09:05');
INSERT INTO `verificationcode` VALUES ('8757511d-572e-4b79-b552-21ede52d7eaf', '1', '70082', 'da8adcf2-1c5b-4118-9d62-cd69d31602bd', '70082', '2015-11-23 14:47:57');
INSERT INTO `verificationcode` VALUES ('a3a377a9-7ad6-44e3-a154-2f50dcf9ddff', '1', '83293', '147780a9-24de-4088-ba1a-906786bbabbb', '83293', '2015-11-12 11:09:14');
INSERT INTO `verificationcode` VALUES ('e40ad1bf-0270-4acb-8fb3-05882353fd39', '1', '84790', '9ffa65d6-fef8-4049-9f28-e9603b095df9', '84790', '2015-10-27 18:15:06');
