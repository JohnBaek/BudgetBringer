import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {CommonColumnDefinitions} from "../../../shared/grids/common-grid-column-definitions";

/**
 * User Management Grid Model
 */
export class UserManagementGridData extends CommonGridModel{
  /**
   * Constructor
   */
  constructor() {
    super();
    this.columDefined = [
      CommonColumnDefinitions.createColumnDefinitionForTextFilter(200, "displayName", "사용자명"),
      CommonColumnDefinitions.createColumnDefinitionForTextFilter(200, "loginId", "로그인 아이디"),
    ]
  }
}

