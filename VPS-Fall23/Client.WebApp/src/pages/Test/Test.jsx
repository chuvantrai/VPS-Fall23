import classNames from 'classnames/bind';
import styles from './Test.module.scss';
import { Button, Form, Input, Modal, Select } from 'antd';
import { useState } from 'react';
import TextArea from 'antd/es/input/TextArea.js';
import optionsReportType from '@/helpers/optionsReportType.js';
import reportTypeEnum from '@/helpers/reportTypeEnum.js';
import GetAccountJwtModel from '@/helpers/getAccountJwtModel.js';
import userRoleEnum from '@/helpers/userRoleEnum.js';
import reportServices from '@/services/reportServices.js';
import ModalReport from '@/components/modalDialogs/modalReport/index.js';
import { QuestionCircleOutlined } from '@ant-design/icons';


const cx = classNames.bind(styles);
const contentBtnReport = (
  <>
    <Button className={cx('bg-[#1890FF]')} type='primary'>Tạo báo cáo</Button>
  </>
);

function Test() {


  return (
    <>
      <div
        className={cx('w-full pl-[20px] pt-[20px] pr-[20px] min-h-[calc(100vh-250px)] mt-[20px] page-account-profile')}>
        <ModalReport contentBtn ={contentBtnReport}/>
      </div>
    </>
  );
}

export default Test;