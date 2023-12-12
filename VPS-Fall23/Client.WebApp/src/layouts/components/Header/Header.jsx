import classNames from 'classnames/bind';
import { Header as AntdHeader } from 'antd/es/layout/layout';
import styles from './Header.module.scss';
import { Button, Col, Menu, Row} from 'antd';
import HeaderLeft from '@/layouts/components/Header/Left';
import HeaderRight from '@/layouts/components/Header/Right';
import HeaderCenter from './Center/HeaderCenter';
import ModalReport from "@/components/modalDialogs/modalReport/index.js";
import {QuestionCircleOutlined} from "@ant-design/icons";
import userRoleEnum from "@/helpers/userRoleEnum.js";
import reportTypeEnum from "@/helpers/reportTypeEnum.js";
import GetAccountJwtModel from "@/helpers/getAccountJwtModel.js";
const cx = classNames.bind(styles);

function Header() {
  const contentBtnReport = (
      <>
          <QuestionCircleOutlined className={'text-[2.4rem] cursor-pointer'} />
      </>
  );
  const account = GetAccountJwtModel();
  return (
      <>
          <AntdHeader className={cx('nav')}>
              <Row className={cx('nav-row')}>
                  <Col md={8} sm={0} xs={0} className={cx('nav-left')}>
                      <HeaderLeft></HeaderLeft>
                  </Col>
                  <Col md={8} sm={24} style={{textAlign: "center"}}>
                      <HeaderCenter></HeaderCenter>
                  </Col>
                  <Col md={8} sm={24}>
                      <HeaderRight></HeaderRight>
                  </Col>
              </Row>
          </AntdHeader>
          {
              account?.RoleId === userRoleEnum.OWNER ?
                  <div className={'flex justify-center fixed text-[#646464] w-[27px] h-[27px] items-center ' +
                      'shadow-[1px_1px_5px_rgba(0,0,0,0.3)] pt-[2px] rounded-[50%] right-[22px] bottom-[30px]'}>
                      <ModalReport contentBtn={contentBtnReport}/>
                  </div>
                  : <></>
          }
      </>
  );
}

export default Header;
